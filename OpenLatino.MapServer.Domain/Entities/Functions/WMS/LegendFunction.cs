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
    public class LegendFunction : IWMSFunction
    {
        private readonly string Legend = "GetGraphicLegend";

        public IResponse Response { get; set; }

        public IEnumerable<IProviderService> Providers { get; set; }
        public string requestName { get => (string)Legend ;}
        public string[] responseFormat { get => new string[] { "image/png" }; }

        public LegendFunction()
        {}

        public virtual bool CanResolve(IRequest request)
        {
           //fixed bug here
            bool flag = request.HasParameter("REQUEST") && 
                request["REQUEST"] == requestName && 
                (request.HasParameter("TEMATICLAYERID") && request["TEMATICLAYERID"] == "" || 
                !request.HasParameter("TEMATICLAYERID"));
            return flag;
        }

        public virtual Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            List<LegendObject> results = new List<LegendObject>();

            foreach (var layer in layers)
            {
                //if (workSpace.LayerWorkspaces.Any(lw => lw.LayerId == layer.Id))
                //    results.Add(new LegendObject { Name = c.Style.Name, Color = c.Style.Fill, Line = c.Style.Line });
                //else
                //{
                    var style = layer.VectorStyles.First();
                    results.Add(new LegendObject 
                    { 
                        Name = style.VectorStyle.Name, 
                        Color = style.VectorStyle.Fill,
                        Line = style.VectorStyle.Line
                    });
                //}
                //results.AddRange(
                //    layer.StyleConfiguration.Select(c => new LegendObject { Name = c.Style.Name, Color = c.Style.Fill, Line = c.Style.Line }));
                                
            }
            LegendJsonResponse lResponse = new LegendJsonResponse();
            lResponse.LegendResult = results;
            return Task.FromResult<IResponse>(lResponse);
        }

    }
}
