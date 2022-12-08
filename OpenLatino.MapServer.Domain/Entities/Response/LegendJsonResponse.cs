using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Domain.Utils;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public class LegendJsonResponse : ILegendResponse
    {

        public IEnumerable<LegendObject> LegendResult { get; set; }

        public string contentType => "text/json";

        public LegendJsonResponse()
        {
        }
        public HttpResponseMessage GetHttpResponse()
        {
            throw new NotImplementedException();
        }

        public Stream GetImage()
        {
            throw new NotImplementedException();
        }
        public object GetResponseContent()
        {
            return new JsonContent(LegendResult);
        }

        public bool HasImage()
        {
            return false;
        }

        public bool CanResolve(IRequest request)
        {
            return request.HasParameter("FORMAT") && request["FORMAT"] == contentType;
        }

        public Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace)
        {
            return Task.FromResult<IResponse>(this);
        }
    }
}
