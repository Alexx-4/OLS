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
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public class WMSSpatialFunction : IWMSFunction
    {
        private readonly string SpatialQuery = "SpatialQuery";

        public IResponse Response { get; set; }

        public IEnumerable<IProviderService> Providers { get; set; }

        public IEnumerable<IWMSSpatialQuery> SpatialQuerys { get; set; }

        public string requestName => SpatialQuery;

        public string[] responseFormat => new string[] { "text/json","text/xml"};

        public WMSSpatialFunction()
        {}

        public WMSSpatialFunction(IEnumerable<IWMSSpatialQuery> querys)
        {
            SpatialQuerys = querys;
        }

        //private IEnumerable<Tuple<int, string>> GetGeometries()
        //{
        //    var iter = Request.Body as IEnumerable<SpatialGeoQuery>;
        //    foreach (var item in iter)
        //        yield return new Tuple<int, string>(item.IdGeometry, item.GeometryText);
        //}  

        public bool CanResolve(IRequest request)
        {
            bool flag = request.HasParameter("REQUEST") && request["REQUEST"] == SpatialQuery &&
                   request.HasParameter("LAYERS") && (request.Body != null);
            return flag;
        }

        public async Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            IEnumerable<SpatialGeometriesRequests> geometriesRequested = request.Body as IEnumerable<SpatialGeometriesRequests>;
            if (geometriesRequested.Count() == 0)
            {
                return new SpatialResponse(new List<GeometryWithFeatures>());
            }
            List<string> geotext = new List<string>();
            List<EnclousureType> geoenclousure = new List<EnclousureType>();
            List<string> geosetoper = new List<string>();
            foreach (var item in geometriesRequested)
            {
                geotext.Add(item.GeometryText);
                geoenclousure.Add(item.Clousure);
                geosetoper.Add(item.SetOperation);
            }

            List<GeometryWithFeatures> results = new List<GeometryWithFeatures>();

            foreach (var layer in layers)
            {
                var provider = providers.First(p => p.Layers.Contains(layer));
                IWMSSpatialQuery spatialquery = SpatialQuerys.First(query => query.Provider == provider.GetType());
                //esto es lo nevo
                spatialquery.GeometryText = geotext.ToArray();
                spatialquery.Clousure = geoenclousure.ToArray();
                spatialquery.SetOperation = geosetoper.ToArray();
                SetQuery(request, spatialquery, provider);
                //ver esto aqui info vs alfainfos
                spatialquery.InfoColums = layer.AlfaInfoes.First().Columns;
                var columns = spatialquery.InfoColums.Split(',');
                spatialquery.TypesReturn = Enumerable.Repeat(typeof(string), columns.Count() + 2).ToArray();
                spatialquery.splitOn = spatialquery.ProviderPkField + ',' + spatialquery.ProviderGeoField + "," + spatialquery.InfoColums;

                List<GeometryWithFeatures> resultXLayer = new List<GeometryWithFeatures>();

                WKTReader wkt = new WKTReader();
                List<GeometryWithFeatures> elements = new List<GeometryWithFeatures>();

                foreach (GeometryWithFeatures item in await provider.ExecuteAsync(spatialquery))
                {
                    item.Geom.LayerId = layer.Id; 
                    elements.Add(item);
                    //esto es mio

                    foreach (var elem in elements)
                        if (!resultXLayer.Contains(elem))
                        {
                            elem.LayerId = layer.Id;
                            resultXLayer.Add(elem);
                        }
                }

                results.AddRange(resultXLayer);
            }

            return new SpatialResponse(results);

        }
        private void SetQuery(IRequest request, IWMSSpatialQuery spatial_query, IProviderService provider)
        {
            spatial_query.Crs = request["CRS"].Replace("EPSG:", "");
            spatial_query.Provider = provider.GetType();
            spatial_query.ProviderGeoField = provider.GeoField;
            spatial_query.ProviderPkField = provider.PkField;
            spatial_query.ProviderTable = provider.Table;
            spatial_query.Top = request["TOP"];

        }
    }
}
