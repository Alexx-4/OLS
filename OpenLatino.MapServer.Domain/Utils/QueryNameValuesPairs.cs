using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace OpenLatino.MapServer.Domain.Utils
{
    public static class QueryNameValuesPairs
    {
        public static List<KeyValuePair<string,string>> Get(this HttpRequestMessage request)
        {
            string url = System.Net.WebUtility.UrlDecode(request.RequestUri.ToString());
            //string url = request.RequestUri.ToString();
            var _params = url.Split('?')[1].Split('&');

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < _params.Length; i++)
            {
                var item = _params[i].Split('=');
                if(item.Length > 1)
                    result.Add(new KeyValuePair<string, string>(item[0], item[1]));
            }
            return result;
        }

        public static List<KeyValuePair<string, string>> Get(string url)
        {
            if (url == "")
                return null;

            var _params = url.Split('?')[1].Split('&');

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < _params.Length; i++)
            {
                var item = _params[i].Split('=');
                if(item.Length > 1)
                    result.Add(new KeyValuePair<string, string>(item[0], item[1]));
            }
            return result;
        }
    }
}
