using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Functions.SOP;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Protocol.SOP
{
    public class SOPProtocol : IProtocol
    {
        private readonly string _version = "1.1.3";
        private readonly string _service = "SOP";

        public IEnumerable<ISOPFunction> SOPFunctions { get; set; }

        public SOPProtocol(IEnumerable<ISOPFunction> functions)
        {
            SOPFunctions = functions;
        }
        public bool CanResolve(IRequest request)
        {
            var answer = request.HasParameter("VERSION") && request["VERSION"] == _version && request.HasParameter("SERVICE") && request["SERVICE"] == _service;
            return answer;
        }

        public async Task<IResponse> Resolve(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            return await SOPFunctions.FirstOrDefault(t => t.CanResolve(request)).Process(request, providers, layers, workSpace,Legend);
        }
    }
}