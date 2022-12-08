using System;
using System.Collections.Generic;
using OpenLatino.Core.Domain.Entities;
using System.Linq;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public class WMSDescribeLayersFunction : IWMSFunction
    {
        private readonly string _version = "1.3.0";
        private readonly string _service = "WMS";
        private readonly string GetCapabilities2 = "DescribeLayers";
        public IEnumerable<IProviderService> Providers { get; set; }

        public string requestName => GetCapabilities2;

        public string[] responseFormat => new string[] { "text/json" };

        public WMSDescribeLayersFunction()
        {}

        public bool CanResolve(IRequest request)
        {
            return request.HasParameter("SERVICE") && request["SERVICE"] == _service && request.HasParameter("REQUEST") && request["REQUEST"] == GetCapabilities2 && request["VERSION"] == _version;
        }

        public Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers,IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            foreach (var layer in layers)
            {
                return Task.FromResult<IResponse>(new ColumnsResponse(layer.AlfaInfoes.First().Columns.Split(',')));
            }
            return null;
        }
    }
}
