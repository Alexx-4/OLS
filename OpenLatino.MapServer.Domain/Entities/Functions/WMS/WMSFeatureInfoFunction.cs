using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using OpenLatino.MapServer.Domain.Map.OpenMath;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public class WMSFeatureInfoFunction : IWMSFunction
    {
        private readonly string _version = "1.3.0";
        private readonly string FeatureInfo = "GetFeatureInfo";
        private readonly string _service = "WMS";
        public IEnumerable<IProviderService> Providers { get; set; }
        public IEnumerable<IWMSFeatureInfoQuery> FeatureInfoQuerys { get; private set; }

        public string requestName =>FeatureInfo;

        public string[] responseFormat => new string[] { "text/json" };

        public WMSFeatureInfoFunction()
        {}

        public WMSFeatureInfoFunction(IEnumerable<IWMSFeatureInfoQuery> featureinfoquery)
        {
            FeatureInfoQuerys = featureinfoquery;
        }
        private Point ToPaint(LtnMatrix matup, LtnMatrix matdown, Point p1, Point p2, Point p)
        {
            return (PosToRect(p, p1, p2) >= 0 ? matup : matdown).TransformPoint(p);
        }

        private double PosToRect(Point p, Point p1, Point p2)
        {
            return ((p.Y - p1.Y) * (p2.X - p1.X)) - ((p2.Y - p1.Y) * (p.X - p1.X));
        }

        private void GetCoord(out double x, out double y, IRequest request)
        {
            if (!request.HasParameter("BBOX"))
            {//PEDIDO DONDE SE PASAN DIRECTAMENTE LAS COORDENADAS DEL MUNDO
                x = double.Parse(request["X"]);
                y = double.Parse(request["Y"]);
                return;
            }
            var bboxParam = request["BBOX"].Split(',');
            double minx = double.Parse(bboxParam[0]), miny = double.Parse(bboxParam[1]), maxx = double.Parse(bboxParam[2]), maxy = double.Parse(bboxParam[3]);
            var width = double.Parse(request["WIDTH"]);
            var height = double.Parse(request["HEIGHT"]);
            x = double.Parse(request["X"]);
            y = double.Parse(request["Y"]);

            var matup = new LtnMatrix();
            var matdown = new LtnMatrix();

            Point bbox1 = new Point(0, 0), bbox2 = new Point(0, height), bbox3 = new Point(width, height), bbox4 = new Point(width, 0);
            Point dest1 = new Point(minx, miny), dest2 = new Point(dest1.X, maxy), dest3 = new Point(maxx, maxy), dest4 = new Point(dest3.X, dest1.Y);

            //Configurando matup
            matup.SetERT(bbox1, bbox2, bbox3, dest1, dest2, dest3);

            //Configurando matdown
            matdown.SetERT(bbox2, bbox3, bbox4, dest2, dest3, dest4);

            Func<Point, Point> transformation = p => ToPaint(matup, matdown, bbox1, bbox3, p);
            var pResult = transformation(new Point(x, y));
            x = pResult.X;
            y = pResult.Y;
        }

        public bool CanResolve(IRequest request)
        {
            int tmp;
            //var answ = request.HasParameter("SERVICE") && request["SERVICE"] == _service &&
            //       request.HasParameter("VERSION") && request["VERSION"] == _version &&
            //       request.HasParameter("REQUEST") && request["REQUEST"] == FeatureInfo &&
            //       request.HasParameter("LAYERS") && request.HasParameter("CRS") && (!request.HasParameter("BBOX") || (
            //       request.HasParameter("WIDTH") && int.TryParse(request["WIDTH"], out tmp) && request.HasParameter("HEIGHT") && int.TryParse(request["HEIGHT"], out tmp))) &&
            //       request.HasParameter("FEATURE_COUNT") && request.HasParameter("X") && request.HasParameter("Y");

            // GetFeuturesInfo's FEATURE_COUNT param is not required

            var answ2 = request.HasParameter("SERVICE") && request["SERVICE"] == _service &&
                   request.HasParameter("VERSION") && request["VERSION"] == _version &&
                   request.HasParameter("REQUEST") && request["REQUEST"] == FeatureInfo &&
                   request.HasParameter("LAYERS") && request.HasParameter("CRS") && (!request.HasParameter("BBOX") || (
                   request.HasParameter("WIDTH") && int.TryParse(request["WIDTH"], out tmp) && request.HasParameter("HEIGHT") && int.TryParse(request["HEIGHT"], out tmp))) &&
                   request.HasParameter("X") && request.HasParameter("Y");

            return answ2;
        }
        public async Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            var crs = request["CRS"];
            var feature_count = request.HasParameter("FEATURE_COUNT")?request["FEATURE_COUNT"]:"1";
            var crsInt = int.Parse(crs);
            double x, y;
            GetCoord(out x, out y, request);


            IEnumerable<GeometryWithFeatures> results = new List<GeometryWithFeatures>();
            foreach (var layer in layers)
            {
                IProviderService provider = providers.First(p => p.ID == layer.ProviderInfoId);
                IWMSFeatureInfoQuery featureinfo_query = FeatureInfoQuerys.FirstOrDefault(sp => sp.Provider == provider.GetType());
                featureinfo_query.Crs = crs;
                featureinfo_query.Point = new Point(x, y);
                featureinfo_query.ProviderGeoField = provider.GeoField;
                featureinfo_query.ProviderTable = provider.Table;
                featureinfo_query.ProviderPkField = provider.PkField;
                featureinfo_query.Feature_Count = int.Parse(feature_count);
                //alfainfos vs infos
                featureinfo_query.InfoColums = layer.AlfaInfoes.First().Columns;
                var columns = featureinfo_query.InfoColums.Split(',');
                featureinfo_query.TypesReturn = Enumerable.Repeat(typeof(string), columns.Count() + 2).ToArray();
                featureinfo_query.splitOn = featureinfo_query.ProviderPkField + ',' + featureinfo_query.ProviderGeoField + ',' + featureinfo_query.InfoColums;
                List<GeometryWithFeatures> elementsInLayer = new List<GeometryWithFeatures>();

                foreach (GeometryWithFeatures item in await provider.ExecuteAsync(featureinfo_query))
                {
                    item.Geom.LayerId = layer.Id;
                    item.LayerId = layer.Id;
                    elementsInLayer.Add(item);
                }
                results = results.Concat(elementsInLayer);
            }
           
            return new FeatureResponse(results);
        }
    }
}
