using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys.SOP;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.MapServer.Domain.Map.Render;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Functions.SOP
{
    public class SOPSpatialFunction : ISOPFunction
    {
        private readonly string SpatialQuery = "SpatialQuery";

        public IResponse Response { get; set; }

        public IEnumerable<IProviderService> Providers { get; set; }

        public IEnumerable<ISOPSpatialQuery> SpatialQuerys { get; set; }

        public string requestName => SpatialQuery;

        public SOPSpatialFunction(IEnumerable<ISOPSpatialQuery> querys)
        {
            SpatialQuerys = querys;
        }

        public bool CanResolve(IRequest request)
        {
            bool flag = request.HasParameter("REQUEST") && request["REQUEST"] == SpatialQuery &&
                   //(!request.HasParameter("TOP") || int.TryParse(request["TOP"], out aux)) &&
                   //(!request.HasParameter("SKIP") || int.TryParse(request["SKIP"], out aux)) &&
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

            var bboxComponents = geometriesRequested.ToArray()[0].boundingBox.Split(',');
            var bbox = new Tuple<Point, Point>(
                new Point(double.Parse(bboxComponents[1]), double.Parse(bboxComponents[0])),
                new Point(double.Parse(bboxComponents[3]), double.Parse(bboxComponents[2])));
            

            List<GeometryWithFeatures> results = new List<GeometryWithFeatures>();

            foreach (var layer in layers)
            {
                // if (layer.Id != 3)
                // {
                    var provider = providers.FirstOrDefault(p => p.Layers !=null && p.Layers.Contains(layer));
                    ISOPSpatialQuery spatialquery = SpatialQuerys.First(query => query.Provider == provider.GetType());
                    //esto es lo nevo
                    spatialquery.GeometryText = geotext.ToArray();
                    spatialquery.Clousure = geoenclousure.ToArray();
                    spatialquery.SetOperation = geosetoper.ToArray();
                    SetQuery(request, spatialquery, provider);
                    spatialquery.BoundingBox = bbox;
                    //ver esto aqui info vs alfainfos
                    spatialquery.InfoColums = layer.AlfaInfoes.First().Columns;
                    var columns = spatialquery.InfoColums.Split(',');
                    spatialquery.TypesReturn = Enumerable.Repeat(typeof(string), columns.Count() + 2).ToArray();
                    spatialquery.splitOn = spatialquery.ProviderPkField + ',' + spatialquery.ProviderGeoField + "," + spatialquery.InfoColums;

                    List<GeometryWithFeatures> resultXLayer = new List<GeometryWithFeatures>();

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
           // }


            return new SpatialResponse(results);
        }
        private void SetQuery(IRequest request, ISOPSpatialQuery spatial_query, IProviderService provider)
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