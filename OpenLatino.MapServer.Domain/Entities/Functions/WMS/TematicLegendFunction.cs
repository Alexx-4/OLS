using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public class TematicLegendFunction : LegendFunction
    {
        

        public TematicLegendFunction()
        { }

        public override bool CanResolve(IRequest request)
        {
            return request.HasParameter("REQUEST") && request["REQUEST"] == requestName && (request.HasParameter("TEMATICLAYERID") && request["TEMATICLAYERID"] != "");
        }

        public override Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            
            List<LegendObject> results = new List<LegendObject>();
            List<string> tematicLayersRequested = request["TEMATICLAYERID"].Split(',').ToList();
            List<TematicLayer> tl = new List<TematicLayer>();
            foreach (var layer in layers)
            {
                //if (workSpace.LayerWorkspaces.Any(lw => lw.LayerId == layer.Id))
                //    results.Add(new LegendObject { Name = c.Style.Name, Color = c.Style.Fill, Line = c.Style.Line });
                //else
                //{
                tl = tl.Concat(layer.StyleConfiguration.Where(sc => tematicLayersRequested.Contains(sc.TematicLayerId.ToString())).Select(sc => sc.TematicLayer).ToList()).ToList();
                //}
                //results.AddRange(
                //    layer.StyleConfiguration.Select(c => new LegendObject { Name = c.Style.Name, Color = c.Style.Fill, Line = c.Style.Line }));

            }
            Dictionary<int, TematicLayer> dict = new Dictionary<int, TematicLayer>();
            foreach (var item in tl)
                if (!dict.ContainsKey(item.Id))
                    dict[item.Id] = item;
            foreach (var item in dict)
            {
                var configs = item.Value.StyleConfiguration.Select(sc => sc);
                foreach (var config in configs)
                {
                    string restriction = "";
                    string[] values = config.TematicType.Name.Split(",");
                    for (int i = 0;i <  values.Length;i++)
                    {
                        if (i % 4 == 0 || i % 4 == 2 || i % 4 == 3)
                            restriction += values[i] + " ";
                    }
                    results.Add(new TematicLegendObject
                    {
                        Name = config.Style.Name,
                        Color = config.Style.Fill,
                        Line = config.Style.Line,
                        restrictionName = restriction
                    });
                }
            }
            LegendJsonResponse lResponse = new LegendJsonResponse();
            lResponse.LegendResult = results;
            return Task.FromResult<IResponse>(lResponse);
        }

    }
}
