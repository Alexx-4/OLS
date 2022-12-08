using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OpenLatino.MapServer.Domain.Utils
{
    public static class Xml_helper
    {
        public static string AddLabel(string name, bool open=true, bool single=false ,params Tuple<string, string>[] properties)
        {
            string props = "";
            foreach (var item in properties)
            {
                props += $" {item.Item1}=\"{item.Item2}\"";
            }
            if(single)
                return $"<{name}{props}/>";
            return (open)?$"<{name}{props}>": $"</{name}{props}>";
        }

        public static string AddLabelWithBody(string name, string body, params Tuple<string, string>[] properties)
        {
            string result = AddLabel(name, true, false, properties);
            result += body;
            result += AddLabel(name, false, false, properties);
            return result;
        }

        public static string AddBBox(int west, int east, int south, int north)
        {
            return $"<EX_GeographicBoundingBox><westBoundLongitude>{west.ToString()}</westBoundLongitude><eastBoundLongitude>{east.ToString()}</eastBoundLongitude><southBoundLatitude>{south.ToString()}</southBoundLatitude><northBoundLatitude>{north.ToString()}</northBoundLatitude></EX_GeographicBoundingBox><BoundingBox CRS=\"CRS: 4326\" minx=\"{west.ToString()}\" miny=\"{south.ToString()}\" maxx=\"{east.ToString()}\" maxy=\"{north.ToString()}\"/>";
        }
    }

    public class WMS_Capabilities_Helper
    {
        public dynamic Service;
        public xml_capabilities_helper Capability;
        //public xml_Service_helper service = new xml_Service_helper();

        public WMS_Capabilities_Helper()
        {
            this.Service = new
            {
                Name = "WMS",
                Title = "Spatial Server WMS Service",
                Abstract = "The Spatial Server WMS Service!",
                KeywordList = new
                {
                    Keyword = new string[] { "mapinfo", "geographic", "wms" }
                },
                OnlineResource = "TO IMPLEMENT",
                ContactInformation = new
                {
                    ContactPersonPrimary = "",
                    ContactAddress = ""
                },
                Fees = "NONE",
                AccessConstraints = "NONE",
            };
        }
    }

    public class xml_capabilities_helper
    {
        public dynamic Request;
        public dynamic Exception;
        public dynamic Layer;
    }

    public class xml_Service_helper
    {
        public string Name = "WMS";
        public string Title = "Spatial Server WMS Service";
        public string Abstract = "The Spatial Server WMS Service!";
        public xml_keywordlist_helper KeywordList = new xml_keywordlist_helper();
        public string OnlineResource = "TO IMPLEMENT";
        public string Fees = "NONE";
        public string AccessConstraints = "NONE";

        public class xml_keywordlist_helper
        {
            public string[] keyworld = new string[] { "mapinfo", "geographic", "wms" };
            public class xml_keyworld_helper
            {
                public string keyworld;
            }
        }
    }

    public class xml_layers_helper
    {
        public string Title;
        public List<string> CRS;
        public Dictionary<string, string> EX_GeographicBoundingBox;
        public string BoundingBox = "@CRS=\"CRS:84\" minx=\"-180.0\" miny=\"-90.0\" maxx=\"180.0\" maxy=\"90.0\"";
        public List<xml_style_helper> Style;
        public xml_layers_helper Layer;
        [JsonProperty(PropertyName ="a")]
        public string abc = "test";
        //[JsonProperty(PropertyName = "a")]
        //public string b = "test2";
    }

    public class xml_style_helper
    {
        public string Name;
        public string Title;
        public string Abstract;
    }
}
