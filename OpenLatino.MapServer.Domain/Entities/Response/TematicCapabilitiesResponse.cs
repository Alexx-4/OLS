using Newtonsoft.Json;
using OpenLatino.MapServer.Domain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using OpenLatino.MapServer.Domain.Entities.Functions.WMS;
using System.Dynamic;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.MapServer.Domain.Entities.Response
{
    public class TematicCapabilitiesResponse : CapabilitiesResponse
    {
        public TematicCapabilitiesResponse(IEnumerable<object> layersInfo, SortedSet<string> functions, List<Dictionary<string, object>> tematics) : base(layersInfo, functions, tematics)
        { }

        public override object GetResponseContent()
        {
            string xmlResult = "";


            xmlResult += "<?xml version=\"1.0\" encoding=\"utf-16\"?>";
            xmlResult += Xml_helper.AddLabel("WMS_TematicCapabilities", true, false, new Tuple<string, string>("version", "1.3.0"), new Tuple<string, string>("schemaLocation", "http://www.opengis.net/wms http://schemas.opengis.net/wms/1.3.0/tematicCapabilities_1_3_0.xsd"));


            xmlResult += Xml_helper.AddLabel("TEMATIC_DATA");
            foreach (var tematic in layersInfo)
            {
                xmlResult += Xml_helper.AddLabel("Tematic");
                foreach (var item in tematic)
                {
                    xmlResult += Xml_helper.AddLabel(item.Key);
                    xmlResult += item.Value;
                    xmlResult += Xml_helper.AddLabel(item.Key, false);
                }
                xmlResult += Xml_helper.AddLabel("Tematic", false);
            }

            xmlResult += Xml_helper.AddLabel("TEMATIC_DATA", false);


            xmlResult += Xml_helper.AddLabel("WMS_TematicCapabilities", false);


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlResult);

            //code below is used to correct xml indent
            StringWriter sw = new StringWriter();
            doc.Save(sw);
            string xml = sw.ToString();

            return new StringContent(xml, Encoding.UTF8, "text/xml");
        }
    }
}