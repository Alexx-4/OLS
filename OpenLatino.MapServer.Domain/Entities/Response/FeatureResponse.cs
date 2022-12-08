using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using Newtonsoft.Json;
using OpenLatino.MapServer.Domain.Utils;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public class FeatureResponse : IResponse
    {
        public IEnumerable<GeometryWithFeatures> FeatureInfoResult { get; set; }
        public string contentType => "text/json";

        public FeatureResponse(IEnumerable<GeometryWithFeatures> Result)
        {
            FeatureInfoResult = Result;
        }

        public Stream GetImage()
        {
            throw new NotImplementedException();
        }

        public object GetResponseContent()
        {
            FeatureInfoResponse result = new FeatureInfoResponse()
            {
                fields = FeatureInfoResult.Select(i => new
                {
                    IdGeometry = i.Id,
                    Layer = i.LayerId,
                    GeometryText = i.GeometryText,
                    Features = i.Features
                }).ToArray()
            };

            //var strResult = JsonConvert.SerializeObject(result);

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
