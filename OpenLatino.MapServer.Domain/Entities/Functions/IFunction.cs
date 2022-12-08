using System.Collections.Generic;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;

namespace OpenLatino.MapServer.Domain.Entities.Functions
{
    public interface IFunction
    {
        string requestName { get; }
        bool CanResolve(IRequest request);
        Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend);
    }
}
