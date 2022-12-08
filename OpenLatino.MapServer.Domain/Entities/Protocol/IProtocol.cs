using System.Collections.Generic;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;


namespace OpenLatino.MapServer.Domain.Entities.Protocol
{
    public interface IProtocol
    {
        bool CanResolve(IRequest request);

        Task<IResponse> Resolve(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend);
    }
}
