using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Utils;


namespace OpenLatino.MapServer.Domain.Entities.Requests
{
    public class ImsRequest : IRequest
    {
        private readonly Dictionary<string, string> request;
       
        public ImsRequest(IEnumerable<KeyValuePair<string, string>> request)
        {
            if (request != null)
            {
                this.request = request.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
                this.request["WORKSPACE"] = "common";
            }
        }

        public object Body { get; set; }

        public string this[string nameParam] => request.ContainsKey(nameParam) ? request[nameParam] : "";

        public bool HasParameter(string nameParam)
        {
            return request.ContainsKey(nameParam);
        }

        public static implicit operator ImsRequest(HttpRequestMessage request)
        {
            return new ImsRequest(request.Get()); //request.GetQueryNameValuePairs());
        }
    }
}