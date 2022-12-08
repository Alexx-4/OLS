using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Functions.WMS;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;

namespace OpenLatino.MapServer.Domain.Entities.Protocol.WMS
{
    public class WMSProtocol : IProtocol
    {
        private readonly string _version = "1.3.0";
        private readonly string _service = "WMS";
        public IEnumerable<IWMSFunction> WMSfunctions { get; set; }

        public WMSProtocol(IEnumerable<IWMSFunction> functions)
        {
            WMSfunctions = functions;
        }
        public bool CanResolve(IRequest request)
        {            
            return request.HasParameter("SERVICE") && request["SERVICE"] == _service;
        }

        public async Task<IResponse> Resolve(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            return await WMSfunctions.FirstOrDefault(t => t.CanResolve(request)).Process(request, providers, layers, workSpace, Legend);
        }
        //aqui lleno por reflection las herramientas disponibles para WMS
    }
}
