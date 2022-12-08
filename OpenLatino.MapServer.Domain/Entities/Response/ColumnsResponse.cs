using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using Newtonsoft.Json;
using OpenLatino.MapServer.Domain.Utils;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    class ColumnsResponse: IResponse
    {
        public  IEnumerable<string> ColumnResult { get; set; }

        public string contentType => "text/json";

        public ColumnsResponse(IEnumerable<string> Result)
        {
            ColumnResult = Result;            
        }

        public Stream GetImage()
        {
            throw new NotImplementedException();
        }

        public object GetResponseContent()
        {            
            //var strResult = JsonConvert.SerializeObject(ColumnResult.ToArray());
            return new JsonContent(ColumnResult.ToArray());
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
