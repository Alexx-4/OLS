using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;

namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public class WMSTematicCapabilitiesFunction : WMSCapabilitiesFunction
    {
        public WMSTematicCapabilitiesFunction()
        { }
        public WMSTematicCapabilitiesFunction(IEnumerable<IWMSCapabilitiesQuery> capabilitiesQueries) : base(capabilitiesQueries)
        { }

        public override bool CanResolve(IRequest request)
        {
            bool flag = request.HasParameter("SERVICE") && request["SERVICE"] == _service &&
                   request.HasParameter("VERSION") && request["VERSION"] == _version &&
                   request.HasParameter("REQUEST") && request["REQUEST"] == capabilities &&
                   request.HasParameter("TEMATICDATA");
            return flag;
        }

        public override async Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            var provider = providers.First(p => p.ID == -1);
            IWMSCapabilitiesQuery capability_query = capabilitiesQueries.Where(sp => sp.Provider == provider.GetType()).ToArray()[1];
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach (var item in await provider.ExecuteAsync(capability_query))
            {
                result.Add((Dictionary<string, object>)item);

            }

            return new TematicCapabilitiesResponse(result, null, null);
        }
    }
}