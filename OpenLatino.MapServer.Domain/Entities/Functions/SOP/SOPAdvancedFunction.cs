using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.Querys.SOP;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using OpenLatino.MapServer.Domain.Entities.Functions.SOP;
using OpenLatino.MapServer.Domain.Map.OpenMath;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public class SOPAdvancedFunction : ISOPFunction
    {
        private readonly string AdvancedQuery = "AdvancedQuery";

        public IResponse Response { get; set; }

        public IEnumerable<IProviderService> Providers { get; set; }

        public IEnumerable<ISOPAdvancedQuery> AdvancedQuerys { get; set; }

        public string requestName => AdvancedQuery;

        public SOPAdvancedFunction(IEnumerable<ISOPAdvancedQuery> querys)
        {
            AdvancedQuerys = querys;
        }


        public bool CanResolve(IRequest request)
        {
            int aux;
            bool flag = request.HasParameter("REQUEST") && request["REQUEST"] == AdvancedQuery &&
                   //(!request.HasParameter("TOP") || int.TryParse(request["TOP"], out aux)) &&
                   //(!request.HasParameter("SKIP") || int.TryParse(request["SKIP"], out aux)) &&
                   request.HasParameter("LAYERS") && (request.Body != null);
            return flag;
        }

        public async Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {

            IEnumerable<SpatialGeometriesRequests> body = request.Body as IEnumerable<SpatialGeometriesRequests>;
            List<string> properties = new List<string>();
            List<string> values = new List<string>();
            List<string> setOperations = new List<string>();
            List<string> functions = new List<string>();

            if (body.Count() == 0)
            {
                return new AdvancedResponse(null);
            }

            foreach (var item in body)
            {
                properties.Add(item.Property);
                values.Add(item.Value);
                setOperations.Add(item.SetOperation);
                functions.Add(item.Function);
            }



            List<GeometryWithFeatures> results = new List<GeometryWithFeatures>();

            foreach (var layer in layers)
            {
                var provider = providers.First(p => p.Layers !=null && p.Layers.Contains(layer));
                ISOPAdvancedQuery advancedquery = AdvancedQuerys.First(query => query.Provider == provider.GetType());
                //esto es lo nevo
                SetQuery(request, advancedquery, provider);
                advancedquery.Properties = properties.ToArray();
                advancedquery.Values = values.ToArray();
                advancedquery.SetOperations = setOperations.ToArray();
                advancedquery.Functions = functions.ToArray();
                advancedquery.InfoColums = layer.AlfaInfoes.First().Columns;
                var columns = advancedquery.InfoColums.Split(',');
                advancedquery.TypesReturn = Enumerable.Repeat(typeof(string), columns.Count() + 2).ToArray();
                advancedquery.splitOn = advancedquery.ProviderPkField + ',' + advancedquery.ProviderGeoField + "," + advancedquery.InfoColums;

                List<GeometryWithFeatures> resultXLayer = new List<GeometryWithFeatures>();

                WKTReader wkt = new WKTReader();
                List<GeometryWithFeatures> elements = new List<GeometryWithFeatures>();

                foreach (GeometryWithFeatures item in await provider.ExecuteAsync(advancedquery))
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

            return new AdvancedResponse(results);

        }
        private void SetQuery(IRequest request, ISOPAdvancedQuery spatial_query, IProviderService provider)
        {
            spatial_query.Provider = provider.GetType();
            spatial_query.ProviderGeoField = provider.GeoField;
            spatial_query.ProviderPkField = provider.PkField;
            spatial_query.ProviderTable = provider.Table;

        }
    }
}
