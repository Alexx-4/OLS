using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public interface ILegendResponse : IResponse
    {
        bool CanResolve(IRequest request);
        Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace);
    }
}
