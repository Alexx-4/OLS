using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Domain.Map.Render;
using Bitmap = System.Drawing.Bitmap;


namespace TileGenerator.Domain.Entities
{
    public class GenerateTiles
    {
        const string layer = "layer";
        const string fila = "fila";
        const string columna = "column";
        public IEnumerable<IWMSMapQuery> MapQuerys { get; set; }
        public string ConnectionStringMongo { get; set; }
        public double Scale { get; set; }
        public List<string> Layers2 { get; set; }
        public List<Layer> LayersList { get; set; }
        public List<IProviderService> providerList { get; set; }
        public IEnumerable<VectorStyle> StylesList { get; set; }
        string NameDbMongo { get; set; }
        public GenerateTiles(string cs, double scale, List<string> layers, string csM, string DbMongo)
        {
            Scale = scale;
            Layers2 = layers;
            ConnectionStringMongo =csM;
            NameDbMongo = DbMongo;
        }
        private IEnumerable<Element> GetGeometry(Tuple<Point, Point> bbox,string layer)
        {
            IEnumerable<Element> results = new List<Element>();
            foreach (var item in LayersList)
            {
                if (item.Id.ToString() == layer)
                {
                    IProviderService provider = providerList.First(p => p.ID == item.ProviderInfoId);
                    IWMSMapQuery mapQuery = MapQuerys.FirstOrDefault(sp => sp.Provider == provider.GetType());
                    mapQuery.Crs = "4326";
                    mapQuery.BoundingBox = bbox;
                    mapQuery.ProviderGeoField = provider.GeoField;
                    mapQuery.ProviderTable = provider.Table;
                    results = provider.Execute(mapQuery).Cast<Geometry>();

                    foreach (var item1 in results)
                        item1.LayerId = item.Id;
                    break;
                }
                //results = results.Concat(provider_elements);

            }
            return results;

        }
        private LtnMatrix makewin(Point p1, Point p2)
        {
            if ((p1.X != p2.X || p1.Y != p2.Y))
            {
                double difx = p2.X - p1.X;
                double dify = p2.Y - p1.Y;

                OpenLatino.MapServer.Domain.Map.Primitives.Geometry.Point centerWorld = new OpenLatino.MapServer.Domain.Map.Primitives.Geometry.Point(difx / 2 + p1.X, dify / 2 + p1.Y, 0);

                double scalex = (double)(256) / (double)Math.Abs(difx);
                double scaley = (double)(256) / (double)Math.Abs(dify);
                double scale;

                if (scalex > scaley) scale = scaley;
                else scale = scalex;

                if (scale > 10D) scale = 10D;

                LtnMatrix matrix = new LtnMatrix();

                matrix.SetERT(scale, -scale, 0, (double)difx / 2D + (double)p1.X, (double)dify / 2D + (double)p1.Y, 0, (double)128, (double)128, 0);

                return matrix;

            }
            return null;
        }
        public void GeneratingTiles(double scale, Tuple<Point,Point> bbox, List<string> layer)
        {
            // aqui Convertir el bounding box que esta en coordenadas polares a coordenadas cartesianas para OpenLatino 
            foreach (var item in layer)
            {
                List<string> l = new List<string>();
                l.Add(item);
                GenerateTilesForLayer(scale, bbox, l);
            }
        }
        public void GenerateTilesForLayer(double scale, Tuple<Point,Point> bbox, List<string> layers)
        {
            int delta = 10;
            Point min = bbox.Item1;
            Point max = bbox.Item2;
            double scalex = (double)(256) / (double)Math.Abs(max.X - min.X);
            double scaley = (double)(256) / (double)Math.Abs(max.Y - min.Y);
            double scalef;

            if (scalex > scaley) scalef = scaley;
            else scalef = scalex;

            List<Element> geo = GetGeometry(bbox,layers[0]).ToList();
            var size = new int[] { 256, 256 };
            TileRender vp = new TileRender();
            vp.LayerList = LayersList;
            vp.Styles = StylesList;
            vp.LayerRender = new LayerRender();
            Stream image = vp.RenderT(bbox, layers, geo, size);
            var pp = new BinaryReader(image).ReadBytes((int)image.Length);
            string collection = "0";
            int pos = 0;
            int f = 0;
            int cl = 0;
            Bitmap bmp = new Bitmap(image);
            bmp.Save("C:\\TileRender\\L0X0Y0.bmp");
            bmp.Dispose();

            //insertar el tile en la bd demongo
            //MongoDBService mongoService = new MongoDBService();
            //mongoService.InsertTile(layers, pp, collection, pos, f, cl, ConnectionStringMongo, NameDbMongo);

            LtnMatrix initmatrix = makewin(min, max);
            initmatrix.Invert();
            Point p = new Point(0, 0);
            Point p00 = initmatrix.TransformPoint(p);
            p = new Point(0, 255);
            Point p02047 = initmatrix.TransformPoint(p);
            p = new Point(255, 255);
            Point p20472047 = initmatrix.TransformPoint(p);
            p = new Point(255, 0);
            Point p20470 = initmatrix.TransformPoint(p);

            double difx = p20470.X - p02047.X;
            double dify = p20470.Y - p02047.Y;
            double currentScale = scalef;
            int nt = 2;

            double currentTileSizex = difx;
            double currenTileSizey = dify;
            int level = 1;
            int ntilescount = 1;
            int ntiles = 0;
            bool stop = false;
            for (int i = 0; i < (int)scale; i++)
                ntiles += (int)Math.Pow(4, i);
            do
            {
                currentTileSizex = currentTileSizex / (double)2;
                currenTileSizey = currenTileSizey / (double)2;
                for (int i = 0; i <( nt-1 )*delta +1; i++)
                {
                    for (int j = 0; j < ( nt-1 )*delta +1; j++)
                    {
                        ntilescount++;
                        Point tmin, tmax;
                        tmin = new Point((long)(p02047.X + i * currentTileSizex /delta), (long)(p02047.Y + j * currenTileSizey / delta), 0);
                        tmax = new Point((long)(tmin.X + currentTileSizex), (long)(tmin.Y + currenTileSizey), 0);
                        var size4 = new int[] { 256, 256 };
                        var b = new Tuple<Point, Point>(tmin, tmax);

                        if (b.Item1.X == b.Item1.X && b.Item1.Y == b.Item2.Y || b.Item1.X == b.Item2.X && b.Item2.Y == b.Item2.Y || b.Item1.X == b.Item2.X && b.Item1.Y == b.Item2.Y || b.Item2.X == b.Item2.X && b.Item2.Y == b.Item1.Y)
                        {
                            stop = true;
                            break;
                        }

                        List<Element> g = GetGeometry(b,layers[0]).ToList();
                        Stream img = vp.RenderT(b, layers, g, size4);
                        var pp1 = new BinaryReader(img).ReadBytes((int)img.Length);
                        //mongoService.InsertTile(layers, pp1, level.ToString(), pos, i, j, ConnectionStringMongo, NameDbMongo);
                        g.Clear();
                        bmp = new Bitmap(img);
                        bmp.Save("C:\\TileRender\\" + "L" + level + "X" + i + "Y" + j + ".bmp");
                        bmp.Dispose();
                    }
                    if (stop) break;
                }
                
                nt = nt * 2;
                level = level + 1;
                currentScale = (double)256 / currentTileSizex;

            } while (level < scale && !stop);
        }
        
    }
}
