using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Domain.Map.Render;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public class WMSMapFunction : IWMSFunction
    {
        protected readonly string _version = "1.3.0";
        protected readonly string _service = "WMS";
        protected readonly string GetMap = "GetMap";

        public IEnumerable<IWMSMapQuery> MapQuerys { get; set; }

        public virtual string requestName => GetMap;

        public string[] responseFormat => new string[] { "image/png" };

        protected IRender Render;
        protected bool useCache;
        protected string mongoCs;

        public WMSMapFunction()
        {}

        public WMSMapFunction(IEnumerable<IWMSMapQuery> map_querys, IRender render)
        {
            MapQuerys = map_querys;
            Render = render;
            ConfigureMongo();
        }

        protected void ConfigureMongo()
        {
            string cwd = Directory.GetCurrentDirectory();
            if (!File.Exists($"{cwd}\\render_config.json"))
            {
                var json = new { cache_cs = "mongodb://localhost:27017", enableCache = true, my = true };
                var js = new JsonSerializer();
                var sw = new StreamWriter($"{cwd}\\render_config.json");
                js.Serialize(sw, json);
                sw.Close();
            }
            dynamic jsn = JsonConvert.DeserializeObject(File.ReadAllText($"{cwd}\\render_config.json"));
            try { useCache = jsn["enableCache"]; }
            catch { useCache = true; }

            if (useCache == false)
                mongoCs = "";
            else
            {
                try { mongoCs = jsn["cache_cs"]; }
                catch { mongoCs = "mongodb://localhost:27017"; }
            }
        }

        public virtual bool CanResolve(IRequest request)
        {
            return request.HasParameter("SERVICE") && request["SERVICE"] == _service &&
                request.HasParameter("REQUEST") && request["REQUEST"] == GetMap
                && ((request.HasParameter("TEMATICLAYERID") && request["TEMATICLAYERID"] == "") || !request.HasParameter("TEMATICLAYERID"))
                && request.HasParameter("VERSION") && request["VERSION"] == _version && request.HasParameter("LAYERS")
                && request.HasParameter("STYLES") && request.HasParameter("CRS")
                && request.HasParameter("BBOX") && request.HasParameter("WIDTH")
                && request.HasParameter("HEIGHT") && request.HasParameter("FORMAT");
        }

        public virtual async Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            List<string> stylesRequested, layersRequested;
            int[] Size;
            string crs, bg, format;
            Tuple<Point, Point> bbox;
            bool transparent;
            ObtainRequest(request, out stylesRequested, out Size, out crs, out bbox, out bg, out transparent, out layersRequested, out format);

            MapResponse response = new MapResponse();

            if (useCache)
            {                
                var result = await Render.RenderUsingCacheAsync(layersRequested, stylesRequested, bbox, crs, transparent, format, bg, Size, workSpace,mongoCs);
                if (result != null)
                    response.Tile = result;
            }
            if(response.Tile==null || !useCache)
            {
                IEnumerable<Element> elems = await GetGeometry(providers, Render.LayerList, bbox, crs);
                response.Tile = await Render.RenderCaheDisableAsync(elems, layersRequested, stylesRequested, bbox, crs, transparent, format, bg, Size, workSpace,mongoCs);
            }            

            return response;
        }

        protected static void ObtainRequest(IRequest request, out List<string> stylesRequested, out int[] Size, out string crs, out Tuple<Point, Point> bbox, out string bg, out bool transparent, out List<string> layersRequested, out string format)
        {
            stylesRequested = request["STYLES"].Split(',').ToList();
            Size = new int[] { int.Parse(request["WIDTH"]), int.Parse(request["HEIGHT"]) };
            crs = request["CRS"];
            crs = crs.Replace("EPSG:", "");
            var bboxComponents = request["BBOX"].Split(',');
            bbox = new Tuple<Point, Point>(
                new Point(double.Parse(bboxComponents[1]), double.Parse(bboxComponents[0])),
                new Point(double.Parse(bboxComponents[3]), double.Parse(bboxComponents[2])));
            bg = request.HasParameter("BGCOLOR") ? request["BGCOLOR"] : "";
            transparent = request.HasParameter("TRANSPARENT") ? bool.Parse(request["TRANSPARENT"]) : false;
            layersRequested = request["LAYERS"].Split(',').ToList();
            format = request["FORMAT"];
        }

        protected async Task<IEnumerable<Element>> GetGeometry(IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, Tuple<Point, Point> bbox, string crs)
        {
            List<Element> results = new List<Element>();
            foreach (var layer in layers)
            {
                IProviderService provider = providers.FirstOrDefault(p => p.ID == layer.ProviderInfoId);
                IWMSMapQuery mapQuery = MapQuerys.FirstOrDefault(sp => sp.Provider == provider.GetType());
                mapQuery.Crs = crs;
                mapQuery.BoundingBox = bbox;
                mapQuery.ProviderGeoField = provider.GeoField;
                mapQuery.ProviderTable = provider.Table;
                mapQuery.InfoColums = layer.AlfaInfoes.First().Columns;

                var columns = mapQuery.InfoColums.Split(',');
                mapQuery.ProviderPkField = provider.PkField;
                mapQuery.TypesReturn = Enumerable.Repeat(typeof(string), columns.Count() + 2).ToArray();
                mapQuery.splitOn = mapQuery.ProviderPkField + ',' + mapQuery.ProviderGeoField + "," + mapQuery.InfoColums;

                List<Geometry> geometrys = new List<Geometry>();
                var temp = (await provider.ExecuteAsync(mapQuery)).Cast<GeometryWithFeatures>();
                foreach (var gwf in temp)
                {
                    gwf.Geom.LayerId = layer.Id;
                    geometrys.Add(gwf.Geom);
                }

                results.AddRange(geometrys);

            }
            return results;

        }
    }
}
