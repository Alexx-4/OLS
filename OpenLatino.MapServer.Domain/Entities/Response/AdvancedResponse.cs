using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Utils;
using Newtonsoft.Json;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    class AdvancedResponse : IResponse
    {
        public IEnumerable<GeometryWithFeatures> AdvancedResult { get; set; }

        public string contentType => "text/json";

        public AdvancedResponse(IEnumerable<GeometryWithFeatures> Result)
        {
            AdvancedResult = Result;
        }

        public Stream GetImage()
        {
            throw new NotImplementedException();
        }

        public object GetResponseContent()
        {
            var result = AdvancedResult.Select(i => new
            {
                IdGeometry = i.Id,
                Layer = i.LayerId,
                GeometryText = i.GeometryText,
                Features = i.Features
            }).ToArray();

            return new JsonContent(result);
        }

        public object GetResponse()
        {
            throw new NotImplementedException();
        }

        public bool HasImage()
        {
            return false;
        }
    }
}
