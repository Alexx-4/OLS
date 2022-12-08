#define TEST
using OpenLatino.MapServer.Domain.Map.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brushes = System.Drawing.Brushes;
using Image = System.Drawing.Image;
using Color = System.Drawing.Color;
using SolidBrush = System.Drawing.SolidBrush;
using OpenLatino.MapServer.Domain.Map.Primitives;
using MongoDB.Driver;
using MongoDB.Bson;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using Point = OpenLatino.MapServer.Domain.Map.Primitives.Geometry.Point;
using OpenLatino.Core.Domain;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System.Runtime.Serialization.Formatters.Binary;
using OpenLatino.MapServer.Domain.Map.Filters;
using OpenLatino.MapServer.Domain.Map.Filters.Helpers;
//using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json;

namespace OpenLatino.MapServer.Domain.Map.Render
{
    public class TileRender : IRender
    {
        public IEnumerable<Layer> LayerList { get; set; }
        public IEnumerable<TematicLayer> tematicLayerList { get; set; }
        public IEnumerable<VectorStyle> Styles { get; set; }
        public Tuple<Point, Point> MaxBoundingBox { get; set; }
        public ILayerRender LayerRender { get; set; }
        public IEnumerable<Layer> _tematicLayersRequested { get; set; }
        

        public TileRender()
        {
        }
        
        public Stream RenderT(Tuple<Point, Point> bbox, List<string> layersRequested, List<Element> elements, int[] Size)
        {
            #region Sacando toda la información que hace falta que está en request
            bool transparent = true;
            string bg = "";
            List<string> stylesRequested = new List<string>();

            #endregion

            #region Realizando comprobaciones

            bool noLayersRequested = layersRequested.Count() == 0 || (layersRequested.Count() == 1 && string.Equals(layersRequested.First(), ""));
            if (noLayersRequested)
            {
                layersRequested = LayerList.Select(l => l.Id.ToString()).ToList();
                //stylesRequested = LayerList.Select(l => l.Styles.First().Name).ToList();
                LayerList = LayerList.Where(ll => layersRequested.Any(lr => lr == ll.Id.ToString())).ToList();
            }

            bool noStylesRequested = stylesRequested.Count() == 0 || (stylesRequested.Count() == 1 && string.Equals(stylesRequested.First(), ""));
            if (noStylesRequested)
            {
                stylesRequested.Clear();
                foreach (var item in LayerList)
                    if (layersRequested.Any(lr => string.Equals(lr, item.Id.ToString())))
                    {
                        //stylesRequested.Add(item.Styles.First().Name);
                    }
                LayerList = LayerList.Where(ll => layersRequested.Any(lr => lr == ll.Id.ToString())).ToList();
            }

            //if (!Enumerable.Zip(stylesRequested, LayerList,
            //    (s, l) => new Tuple<string, Layer>(s, l)
            //    ).All(t => t.Item2.Styles.Any(s => s.Name == t.Item1)))
            //{
            //    throw new NoStyle();
            //}

            #endregion

            #region Creando los layerRenders en dependenca de la cantidad de layerRequested existentes

            #region Configurando la función de transformación de LatLong a XY

            var matup = new OpenMath.LtnMatrix();
            var matdown = new OpenMath.LtnMatrix();

            Point bbox1 = bbox.Item1, bbox2 = new Point(bbox1.X, bbox.Item2.Y), bbox3 = bbox.Item2, bbox4 = new Point(bbox3.X, bbox1.Y);
            Point dest1 = new Point(0, 0), dest2 = new Point(0, Size[1]), dest3 = new Point(Size[0], Size[1]), dest4 = new Point(Size[0], 0);
            //if (bbox1.X==bbox2.X&& bbox1.Y==bbox2.Y|| bbox2.X == bbox3.X && bbox2.Y == bbox3.Y|| bbox1.X == bbox3.X && bbox1.Y == bbox3.Y|| bbox3.X == bbox4.X && bbox3.Y == bbox4.Y)
            //{
            //    int a = 1;
            //}
            //Configurando matup
            matup.SetERT(bbox1, bbox2, bbox3, dest1, dest2, dest3);

            //Configurando matdown
            matdown.SetERT(bbox2, bbox3, bbox4, dest2, dest3, dest4);



            var layerRenders = new List<ILayerRender>();
            var layerRendersDic = new Dictionary<string, ILayerRender>();
            for (int i = 0; i < layersRequested.Count(); i++)
            {
                var newLayerRender = LayerRender.Clone() as ILayerRender;
                newLayerRender.MatrixTransformationUp = matup;
                newLayerRender.MatrixTransformationDown = matdown;
                newLayerRender.Format = "png";
                layerRenders.Add(newLayerRender);
                layerRendersDic[layersRequested[i]] = newLayerRender;
            }

            #endregion

            foreach (var l in layerRenders)
            {
                l.BBOX = bbox;
                l.Size = Size;
                l.Transparent = true;
            }

            //setear estilos de los layerRenders
            foreach (var item in layerRendersDic)
            {
                var idLayer = item.Key;
                var currentLayerRender = item.Value;
                //var currentLayerStyles = LayerList.First(l => string.Equals(l.UniversalId.ToString(), idLayer)).Styles;
                //foreach (var layerStyle in currentLayerStyles)
                //    if (stylesRequested.Any(s => s == layerStyle.Name))
                //    {
                //        currentLayerRender.Style = layerStyle;
                //        break;
                //    }
            }

            var lastLayer = layerRendersDic[layersRequested.Last()];
            if (!(lastLayer.Transparent = transparent))
                lastLayer.Background = bg == "" ? Brushes.Blue : new SolidBrush(Color.FromName(bg));

            #endregion

            #region Rendereado

            foreach (var item in elements)
            {
                //TODO: Revisar cómo hacer la distribución de elementos en capas en caso de que acepte que un elemento pueda estar en más de una capa
                var layerKey = layersRequested.FirstOrDefault(layerName => string.Equals(layerName, item.LayerId.ToString()));
                var layer = layerKey == null ? null : layerRendersDic[layerKey];
                if (layer == null)
                    throw new NoLayer();
                else
                    layer.AddElement(item);
            }

            var lastLayerRequested = layersRequested.Last();
            IMapImage result = layerRendersDic[lastLayerRequested].GetImage();
            foreach (var layer in layersRequested.TakeWhile(l => l != lastLayerRequested).Where(l => layerRendersDic[l].ImageRendered))
                result.OverlapImage(layerRendersDic[layer].GetImage());

#if TEST
            Task.Factory.StartNew(() =>
            {
                foreach (var item in layerRendersDic.Values)
                    item.Dispose();
                GC.Collect();
            });
#endif

            return result.GetImage();

            #endregion
        }

        private List<Tuple<string, VectorStyle>> GetLayersStyles(List<string> layersRequested, List<string> stylesRequested, WorkSpace workSpace)
        {
            var result = new List<Tuple<string, VectorStyle>>();
            for (int i = 0; i < layersRequested.Count; i++)
            {
                var layer = LayerList.First(l => string.Equals($"{l.Id}", layersRequested[i]));
                if (stylesRequested != null)
                {
                    //Comprobar que la capa solicitada posee el estilo solicitado
                    var style = layer.VectorStyles.FirstOrDefault(vs => (vs.VectorStyleId).ToString() == stylesRequested[i]);
                    if (style == null)
                        throw new NoStyle(stylesRequested[i], layersRequested[i]);
                    //Agregar el estilo
                    result.Add(new Tuple<string, VectorStyle>(layersRequested[i], Styles.FirstOrDefault(s => s.Id == style.VectorStyleId)));
                }
                else
                {
                    var layerWorkspaceStyle = workSpace.LayerWorkspaces.Where(lw => lw.LayerId == int.Parse(layersRequested[i])).Select(lw => lw.Style).FirstOrDefault();
                    result.Add(new Tuple<string, VectorStyle>(layersRequested[i], layerWorkspaceStyle ?? Styles.FirstOrDefault(s => s.Id == layer.VectorStyles.First().VectorStyleId)));
                }
            }
            return result;
        }

        private void TestRequest(List<string> layersRequested, ref List<string> stylesRequested, WorkSpace workSpace)
        {
            bool noLayersRequested = layersRequested.Count == 0 || (layersRequested.Count == 1 && string.Equals(layersRequested.First(), ""));
            if (noLayersRequested)
                throw new NoLayerRequested();

            bool noStyleSpecifield = stylesRequested == null || (stylesRequested.Count == 1 && string.Equals(stylesRequested.First(), ""));
            //if (!noStyleSpecifield && stylesRequested.Count != layersRequested.Count)
            //    throw new InvalidNumberOfStyles();
            if (noStyleSpecifield)
                stylesRequested = null;
        }

        private string getMapStyleId(List<string> layersRequested, List<string> stylesRequested, WorkSpace workSpace)
        {
            var layers_styles = GetLayersStyles(layersRequested, stylesRequested, workSpace);
            var layers_styles_id = "";
            foreach (var item in layers_styles.OrderBy(l => l.Item1))//si no se ordenan pueden resultar muchas comb
            {
                layers_styles_id += $"l{item.Item1}_s{item.Item2.Id}_";
            }
            return layers_styles_id;
        }

        public async Task<IMapImage> RenderCaheDisableAsync(IEnumerable<Element> elems, List<string> layersRequested, List<string> stylesRequested, Tuple<Point, Point> bbox, string crs, bool transparent, string format, string bg, int[] size, WorkSpace workSpace = null, string mongoCs = "")
        {
            TestRequest(layersRequested, ref stylesRequested, workSpace);

            #region Creando los layerRenders en dependencia de la cantidad de layerRequested existentes

            #region Configurando la función de transformación de LatLong a XY

            var matup = new OpenMath.LtnMatrix();
            var matdown = new OpenMath.LtnMatrix();

            Point bbox1 = bbox.Item1, bbox2 = new Point(bbox1.X, bbox.Item2.Y), bbox3 = bbox.Item2, bbox4 = new Point(bbox3.X, bbox1.Y);
            Point dest1 = new Point(0, 0), dest2 = new Point(0, size[1]), dest3 = new Point(size[0], size[1]), dest4 = new Point(size[0], 0);

            //Configurando matup
            matup.SetERT(bbox1, bbox2, bbox3, dest1, dest2, dest3);

            //Configurando matdown
            matdown.SetERT(bbox2, bbox3, bbox4, dest2, dest3, dest4);

            var layerRenders = new Dictionary<string, ILayerRender>();
            foreach (var layerId in layersRequested)
            {
                var newLayerRender = LayerRender.Clone() as ILayerRender;
                newLayerRender.MatrixTransformationUp = matup;
                newLayerRender.MatrixTransformationDown = matdown;
                newLayerRender.Format = format;
                newLayerRender.BBOX = bbox;
                newLayerRender.Size = size;
                newLayerRender.Transparent = true;

                layerRenders[layerId] = newLayerRender;
            }

            #endregion

            //Configurar LayerRenders
            int pos = 0;
            var layerStyles = GetLayersStyles(layersRequested, stylesRequested, workSpace);
            foreach (var item in layerRenders)
            {
                var layerId = item.Key;
                var layerRender = item.Value;

                var layer = LayerList.First(l => string.Equals($"{l.Id}", layerId));

          

                layerRender.DefaultVectorStyle = layerStyles[pos].Item2;
                layerRender.CustomStyle = GetGeometryStyle;
                Func<Geometry, bool> f = x => false;
                VectorStyle v = layerRender.DefaultVectorStyle;
                layerRender.Filters = new List<KeyValuePair<Func<Geometry, bool>, VectorStyle>>() { new KeyValuePair<Func<Geometry, bool>, VectorStyle>(f, v) };
                pos++;
            }

            var lastLayer = layerRenders[layersRequested.Last()];
            if (!(lastLayer.Transparent = transparent))
                lastLayer.Background = bg == "" ? Brushes.Blue : new SolidBrush(Color.FromName(bg));

            #endregion

            #region Rendereado

            foreach (var item in elems)
            {
                //TODO: Revisar cómo hacer la distribución de elementos en capas en caso de que acepte que un elemento pueda estar en más de una capa
                var layerKey = layersRequested.FirstOrDefault(layerName => string.Equals(layerName, $"{item.LayerId}"));
                var layerRender = layerKey == null ? null : layerRenders[layerKey];
                if (layerRender == null)
                    throw new NoLayer();
                else
                    layerRender.AddElement(item);
            }

            var lastLayerRequested = layersRequested.Last();
            IMapImage result = layerRenders[lastLayerRequested].GetImage();
            foreach (var layer in layersRequested.TakeWhile(l => l != lastLayerRequested).Where(l => layerRenders[l].ImageRendered))
                result.OverlapImage(layerRenders[layer].GetImage());

#if TEST
            await Task.Factory.StartNew(() =>
            {
                foreach (var item in layerRenders.Values)
                    item.Dispose();
                GC.Collect();
            });
#endif
            #endregion

            #region Add to cache 
            if (mongoCs != "")
            {
                var DB = new MongoClient(mongoCs);//("mongodb://localhost:27017");
                var db = DB.GetDatabase("cache");
                var c = db.GetCollection<BsonDocument>("cache");
                //
                var layers_styles_id = getMapStyleId(layersRequested, stylesRequested, workSpace);
                //
                Stream salva = result.GetImage();
                var pp1 = new BinaryReader(salva).ReadBytes((int)salva.Length);
                salva.Position = 0;
                var d1b = new BsonDocument
                            {{ "layers_styles_id",layers_styles_id},
                             {"minx",bbox.Item1.X},
                             {"miny",bbox.Item1.Y},
                             {"maxx",bbox.Item2.X},
                              {"maxy",bbox.Item2.Y},
                             {"tile",pp1}};
                await c.InsertOneAsync(d1b);
            }
            #endregion

            //Image image = Image.FromStream(result.GetImage());
            //image.Save("C:\\Users\\User1\\Desktop\\Tesis\\example.png", ImageFormat.Png);

            return result;
        }


        public async Task<IMapImage> RenderUsingCacheAsync(List<string> layersRequested, List<string> stylesRequested, Tuple<Point, Point> bbox, string crs, bool transparent, string format, string bg, int[] size, WorkSpace workSpace, string mongoCs = "mongodb://localhost:27017")
        {
            TestRequest(layersRequested, ref stylesRequested, workSpace);

            #region busca en cache o genera
            var DB = new MongoClient(mongoCs);//("mongodb://localhost:27017");
            var db = DB.GetDatabase("cache");
            var c = db.GetCollection<BsonDocument>("cache");
            //
            var layers_styles_id = getMapStyleId(layersRequested, stylesRequested, workSpace);
            //
            var filter = Builders<BsonDocument>.Filter.And(new List<FilterDefinition<BsonDocument>>()
                {
                   //Builders<BsonDocument>.Filter.Eq("layers",lay),
                   Builders<BsonDocument>.Filter.Eq("layers_styles_id", layers_styles_id),
                   Builders<BsonDocument>.Filter.Eq("minx",bbox.Item1.X),
                   Builders<BsonDocument>.Filter.Eq("miny", bbox.Item1.Y),
                   Builders<BsonDocument>.Filter.Eq("maxx",bbox.Item2.X),
                   Builders<BsonDocument>.Filter.Eq("maxy", bbox.Item2.Y)
                });
            var result = (await c.FindAsync(filter)).ToList();

            //existe en cache
            if (result.Count > 0)
            {
                #region existe en cache
                var t = result[result.Count - 1]["tile"].AsByteArray;
                MemoryStream ms = new System.IO.MemoryStream(t, false);
                var bmp2 = new System.Drawing.Bitmap(size[0], size[1]);
                bmp2.Save(ms, GetFormat(format));
                return new MapImage(ms);
                #endregion
            }

            return null;
            #endregion
        }

        //public bool TestingFilters(Geometry g)
        //{
        //    if (!(g.Attributes is null))
        //    {
        //        foreach (var item in g.Attributes)
        //        {
        //            if (item.Key == "type" && (string)item.Value == "hospital")
        //                return true;
        //        }
        //    }
        //    return false;
        //}

        private ImageFormat GetFormat(string Format)
        {
            if (Format.Contains("png")) return ImageFormat.Png;
            else if (Format.Contains("jpeg")) return ImageFormat.Jpeg;
            else throw new InvalidOperationException("Invalid format");
        }

        private VectorStyle GetGeometryStyle(Geometry geometry, IEnumerable<KeyValuePair<Func<Geometry, bool>, VectorStyle>> filters)
        {
            if (geometry.Attributes is null)
                return null;
            
            foreach (var pair in filters)
            {
                var filter = pair.Key;
                if (!filter(geometry))
                    continue;

                var style = pair.Value;
                return style;
            }

            return null;
        }

        #region TematicTileRender

        public void UpdateLayerList(List<string> tematicLayersRequested)
        {
            List<TematicLayer> tematicLayerInstance = new List<TematicLayer>();
            foreach (var item in tematicLayersRequested)
                tematicLayerInstance.Add(tematicLayerList.First(tl => string.Equals($"{tl.Id}", item)));
            List<Layer> temp1 = new List<Layer>();
            foreach (var item in tematicLayerInstance)
                temp1 = temp1.Concat(item.StyleConfiguration.Select(sc => sc.Layer).Distinct().ToList()).ToList();
            this.LayerList = temp1.Concat(this._tematicLayersRequested);
        }
        private List<TematicLayer> GetTematicConfiguration(List<string> tematicLayersRequested, out List<string> layersRequested,out List<string> stylesRequested)
        {
            List<TematicLayer> tematicLayerInstance = new List<TematicLayer>();
            foreach (var item in tematicLayersRequested)
                tematicLayerInstance.Add(tematicLayerList.First(tl => string.Equals($"{tl.Id}", item)));
            List<string> temp1 = new List<string>();
            List<string> temp2 = new List<string>();
            foreach (var item in tematicLayerInstance)
            {
                temp1 = temp1.Concat(item.StyleConfiguration.Select(sc => sc.Layer).Select(l => l.Id.ToString()).ToList()).ToList();
                temp2 = temp2.Concat(item.StyleConfiguration.Select(sc => sc.Style).Select(s => s.Id.ToString()).ToList()).ToList();
            }
            layersRequested = temp1;
            stylesRequested = temp2;
            return tematicLayerInstance;
        }

        
        public async Task<IMapImage> TematicRenderCaheDisableAsync(IEnumerable<Element> elems, List<string> layersRequested, List<string> tematicLayersRequested, List<string> stylesRequested, Tuple<Point, Point> bbox, string crs, bool transparent, string format, string bg, int[] size, WorkSpace workSpace = null, string mongoCs = "")
        {
            List<TematicLayer> tematicLayerInstance = GetTematicConfiguration(tematicLayersRequested, out layersRequested, out stylesRequested);
            TestRequest(layersRequested, ref stylesRequested, workSpace);

            layersRequested = layersRequested.Concat(this._tematicLayersRequested.Select(t => t.Id.ToString())).ToList();

            #region Creando los layerRenders en dependencia de la cantidad de layerRequested existentes

            #region Configurando la función de transformación de LatLong a XY

            var matup = new OpenMath.LtnMatrix();
            var matdown = new OpenMath.LtnMatrix();

            Point bbox1 = bbox.Item1, bbox2 = new Point(bbox1.X, bbox.Item2.Y), bbox3 = bbox.Item2, bbox4 = new Point(bbox3.X, bbox1.Y);
            Point dest1 = new Point(0, 0), dest2 = new Point(0, size[1]), dest3 = new Point(size[0], size[1]), dest4 = new Point(size[0], 0);

            //Configurando matup
            matup.SetERT(bbox1, bbox2, bbox3, dest1, dest2, dest3);

            //Configurando matdown
            matdown.SetERT(bbox2, bbox3, bbox4, dest2, dest3, dest4);

            var layerRenders = new Dictionary<string, ILayerRender>();
            foreach (var layerId in layersRequested)
            {
                var newLayerRender = LayerRender.Clone() as ILayerRender;
                newLayerRender.MatrixTransformationUp = matup;
                newLayerRender.MatrixTransformationDown = matdown;
                newLayerRender.Format = format;
                newLayerRender.BBOX = bbox;
                newLayerRender.Size = size;
                newLayerRender.Transparent = true;

                layerRenders[layerId] = newLayerRender;
            }

            #endregion

            //Configurar LayerRenders
            foreach (var item in layerRenders)
            {
                var layerId = item.Key;
                var layerRender = item.Value;

                var layer = LayerList.First(l => string.Equals($"{l.Id}", layerId));
                var _style = layer.VectorStyles.FirstOrDefault();


                layerRender.DefaultVectorStyle = Styles.FirstOrDefault(s => s.Id == _style.VectorStyleId);
                layerRender.CustomStyle = GetGeometryStyle;

                var conf = layer.StyleConfiguration.Where(sc => tematicLayersRequested.Contains(sc.TematicLayerId.ToString())).ToList();

                layerRender.Filters = conf.Select(
                    config =>
                    new KeyValuePair<Func<Geometry, bool>, VectorStyle>(
                        FilterBuilder.GetFilterFunction<Geometry>(
                            FilterSerializer.ByteArrayToFilterExpression(config.TematicType?.Function)),
                        config.Style)).ToList();

            }

            var lastLayer = layerRenders[layersRequested.Last()];
            if (!(lastLayer.Transparent == transparent))
                lastLayer.Background = bg == "" ? Brushes.Blue : new SolidBrush(Color.FromName(bg));

            #endregion

            #region Rendereado

            foreach (var item in elems)
            {
                //TODO: Revisar cómo hacer la distribución de elementos en capas en caso de que acepte que un elemento pueda estar en más de una capa
                var layerKey = layersRequested.FirstOrDefault(layerName => string.Equals(layerName, $"{item.LayerId}"));
                var layerRender = layerKey == null ? null : layerRenders[layerKey];
                if (layerRender == null)
                    throw new NoLayer();
                else
                    layerRender.AddElement(item);
            }

            var lastLayerRequested = layersRequested.Last();
            IMapImage result = layerRenders[lastLayerRequested].GetImage();
            foreach (var layer in layersRequested.TakeWhile(l => l != lastLayerRequested).Where(l => layerRenders[l].ImageRendered))
                result.OverlapImage(layerRenders[layer].GetImage());

#if TEST
            await Task.Factory.StartNew(() =>
            {
                foreach (var item in layerRenders.Values)
                    item.Dispose();
                GC.Collect();
            });
#endif
            #endregion

            #region Add to cache 
            if (mongoCs != "")
            {
                var DB = new MongoClient(mongoCs);//("mongodb://localhost:27017");
                var db = DB.GetDatabase("cache");
                var c = db.GetCollection<BsonDocument>("cache");
                //
                var layers_styles_id = getMapStyleId(layersRequested, stylesRequested, workSpace);
                //
                Stream salva = result.GetImage();
                var pp1 = new BinaryReader(salva).ReadBytes((int)salva.Length);
                salva.Position = 0;
                var d1b = new BsonDocument
                            {{ "layers_styles_id",layers_styles_id},
                             {"minx",bbox.Item1.X},
                             {"miny",bbox.Item1.Y},
                             {"maxx",bbox.Item2.X},
                              {"maxy",bbox.Item2.Y},
                             {"tile",pp1}};
                await c.InsertOneAsync(d1b);
            }
            #endregion

            //Image image = Image.FromStream(result.GetImage());
            //image.Save("C:\\Users\\Ale\\Desktop\\example.png", ImageFormat.Png);

            return result;
        }

        #endregion
    }
}
