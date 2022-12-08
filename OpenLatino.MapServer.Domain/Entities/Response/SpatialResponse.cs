using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml;
using Newtonsoft.Json;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using Newtonsoft.Json;
using OpenLatino.MapServer.Domain.Utils;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public class SpatialResponse : IResponse
    {

        public IEnumerable<GeometryWithFeatures> SpatialDataResult { get; set; }
        public string contentType => "text/json";
        public SpatialResponse(IEnumerable<GeometryWithFeatures> spatialDataResult)
        {
            SpatialDataResult = spatialDataResult;
        }
        public HttpResponseMessage GetHttpResponse()
        {
            throw new NotImplementedException();
        }

        public Stream GetImage()
        {
            throw new NotImplementedException();
        }

        public JsonReader GetInJson()
        {
            throw new NotImplementedException();
        }

        public XmlDocument GetInXml()
        {
            throw new NotImplementedException();
        }

        public object GetResponseContent()
        {
            var result = SpatialDataResult.Select(i => new
            {
                IdGeometry = i.Id,
                Layer = i.LayerId,
                GeometryText = i.GeometryText,
                Features = i.Features,
                IdContainer = i.IdGeomContainerForSpatialQuery,
                ParentSetOperation = i.ParentSetOperation
            }).ToArray();

            //return new JsonContent(JsonConvert.SerializeObject(result));
            return new JsonContent(result);
        }

        public bool HasImage()
        {
            return false;
        }
    }
}
