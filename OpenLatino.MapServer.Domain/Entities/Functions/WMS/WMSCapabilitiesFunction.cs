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
    public class WMSCapabilitiesFunction : IWMSFunction
    {
        protected readonly string _version = "1.3.0";
        protected readonly string capabilities = "GetCapabilities";
        protected readonly string _service = "WMS";
        protected IEnumerable<IWMSCapabilitiesQuery> capabilitiesQueries;
        public WMSCapabilitiesFunction()
        {}
        public WMSCapabilitiesFunction(IEnumerable<IWMSCapabilitiesQuery> capabilitiesQueries)
        {
            this.capabilitiesQueries = capabilitiesQueries;
        }

        public string requestName => capabilities;

        public string[] responseFormat => new string[] { "text/xml" };

        public virtual bool CanResolve(IRequest request)
        {
            return request.HasParameter("SERVICE") && request["SERVICE"] == _service &&
                   request.HasParameter("VERSION") && request["VERSION"] == _version &&
                   request.HasParameter("REQUEST") && request["REQUEST"] == capabilities &&
                   !request.HasParameter("TEMATICDATA");
        }

        public virtual async Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            var provider = providers.First(p => p.ID == -1);
            IWMSCapabilitiesQuery capability_query = capabilitiesQueries.FirstOrDefault(sp => sp.Provider == provider.GetType());
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach (var item in await provider.ExecuteAsync(capability_query))
            {
                if (workSpace.LayerWorkspaces.Any(lw => lw.LayerId.ToString() == ((Dictionary<string, object>)item)["LayerName"].ToString()))
                    result.Add((Dictionary<string, object>)item);
            }

            IWMSCapabilitiesQuery functionsNamesQuery = capabilitiesQueries.Where(sp => sp.Provider == provider.GetType()).ToArray()[2];
            var functions = await provider.ExecuteAsync(functionsNamesQuery);

            SortedSet<string> enableFunctions = new SortedSet<string>();
            foreach (Dictionary<string,object> item in functions)
            {
                if (workSpace.ServiceFunctions.Any(sf => sf.FunctionId.ToString() == item["Id"].ToString()))
                    enableFunctions.Add(item["Name"].ToString());
            }

            // annadiendo informacion acerca de los tematicos al pedido getcapabilities
            IWMSCapabilitiesQuery tematicDataQuery = capabilitiesQueries.Where(sp => sp.Provider == provider.GetType()).ToArray()[1];
            List<Dictionary<string, object>> tematics = new List<Dictionary<string, object>>();

            foreach (Dictionary<string,object> item in await provider.ExecuteAsync(tematicDataQuery))
            {
                tematics.Add(item);
            }

            return new CapabilitiesResponse(result, enableFunctions, tematics);
        }
    }
}
