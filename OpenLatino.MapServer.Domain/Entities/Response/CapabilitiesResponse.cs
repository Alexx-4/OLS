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
    public class CapabilitiesResponse : IResponse
    {
        public string contentType => "text/xml";
        protected IEnumerable<Dictionary<string, object>> layersInfo;
        private SortedSet<string> functions;
        private SortedSet<string> wrote;

        private List<Dictionary<string, object>> tematics;

        public CapabilitiesResponse(IEnumerable<object> layersInfo, SortedSet<string> functions, List<Dictionary<string, object>> tematics)
        {
            this.layersInfo = (IEnumerable<Dictionary<string, object>>)layersInfo;
            this.wrote = new SortedSet<string>();
            this.functions = functions;
            this.tematics = tematics;
        }

        public Stream GetImage()
        {
            throw new NotImplementedException();
        }

        public virtual object GetResponseContent()
        {
            //SERVICE
            string xmlResult = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";
            xmlResult += Xml_helper.AddLabel("WMS_Capabilities", true, false, new Tuple<string, string>("version", "1.3.0"), new Tuple<string, string>("schemaLocation", "http://www.opengis.net/wms http://schemas.opengis.net/wms/1.3.0/capabilities_1_3_0.xsd"));
            xmlResult += Xml_helper.AddLabel("Service");
            xmlResult += "<Name>WMS</Name><Title>NetCore version of OpenLatino, Spatial Server WMS Service </Title><Abstract>NetCore version of OpenLatino,The Spatial Server WMS Service!</Abstract><KeywordList><Keyword>mapinfo</Keyword><Keyword>geographic</Keyword><Keyword>wms</Keyword></KeywordList>";
            xmlResult += Xml_helper.AddLabel("OnlineResource", false, true, new Tuple<string, string>("type", "simple"), new Tuple<string, string>("href", "https://localhost:44329/api/?"));
            xmlResult += Xml_helper.AddLabel("ContactInformation");
            xmlResult += Xml_helper.AddLabel("ContactPersonPrimary", false, true);
            xmlResult += Xml_helper.AddLabel("ContactAddress", false, true);
            xmlResult += Xml_helper.AddLabel("ContactInformation", false);
            xmlResult += "<Fees>NONE</Fees><AccessConstraints>NONE</AccessConstraints>";
            xmlResult += Xml_helper.AddLabel("Service", false);

            //CAPABILITY
            //REQUEST
            xmlResult += Xml_helper.AddLabel("Capability");
            xmlResult += Xml_helper.AddLabel("Request");

            List<IWMSFunction> requests = getTypeOfRequests();

            foreach (var item in requests)
            {
                if (functions.Contains(item.requestName))
                {
                    xmlResult += Xml_helper.AddLabel(item.requestName);
                    foreach (var form in item.responseFormat)
                    {
                        xmlResult += Xml_helper.AddLabel("Format");
                        xmlResult += form;
                        xmlResult += Xml_helper.AddLabel("Format", false);
                    }
                    xmlResult += Xml_helper.AddLabel("DCPType");
                    xmlResult += Xml_helper.AddLabel("HTTP");
                    xmlResult += Xml_helper.AddLabel("Get");
                    xmlResult += Xml_helper.AddLabel("OnlineResource ", false, true, new Tuple<string, string>("type", "simple"), new Tuple<string, string>("href", $"https://localhost:44329/api/?"));
                    xmlResult += Xml_helper.AddLabel("Get", false);
                    xmlResult += Xml_helper.AddLabel("HTTP", false);
                    xmlResult += Xml_helper.AddLabel("DCPType", false);
                    xmlResult += Xml_helper.AddLabel(item.requestName, false);
                }
            }

            xmlResult += Xml_helper.AddLabel("Request", false);

            ////EXCEPTION

            xmlResult += "<Exception><Format>INIMAGE</Format><Format>BLANK</Format><Format>XML</Format></Exception>";

            ////LAYER

            xmlResult += Xml_helper.AddLabel("Layer", true, false, new Tuple<string, string>("queryable", "false"), new Tuple<string, string>("opaque", "false"), new Tuple<string, string>("noSubsets", "false"), new Tuple<string, string>("fixedWidth", "0"), new Tuple<string, string>("fixedHeight", "0"));
            xmlResult += "<Title>OpenLatinoNetcore Map Server</Title><CRS>CRS:4326</CRS>";
            xmlResult += Xml_helper.AddBBox(-180, 180, -90, 90);

            foreach (var item in layersInfo)
            {
                if (wrote.Contains(item["LayerName"].ToString()))
                    continue;
                wrote.Add(item["LayerName"].ToString());
                xmlResult += Xml_helper.AddLabel("Layer", true, false, new Tuple<string, string>("queryable", "true"), new Tuple<string, string>("opaque", "false"), new Tuple<string, string>("noSubsets", "true"), new Tuple<string, string>("fixedWidth", "0"), new Tuple<string, string>("fixedHeight", "0"));
                xmlResult += Xml_helper.AddLabelWithBody("Name", item["LayerName"].ToString());
                xmlResult += Xml_helper.AddLabelWithBody("Title", (string)item["LayerTitle"]);
                foreach (var itemStyle in layersInfo)//annadir los estilos de esa capa
                {
                    if (itemStyle["LayerName"].ToString() == item["LayerName"].ToString())
                    {
                        xmlResult += Xml_helper.AddLabel("Style");
                        xmlResult += Xml_helper.AddLabelWithBody("Name", itemStyle["StyleName"].ToString());
                        xmlResult += Xml_helper.AddLabelWithBody("Title", (string)itemStyle["StyleTitle"]);
                        xmlResult += Xml_helper.AddLabelWithBody("Abstract", $"EnableOutline is {itemStyle["EnableOutline"].ToString()}, Fill is {(string)itemStyle["Fill"]}, Line is {(string)itemStyle["Line"]}, OutlinePen is {(string)itemStyle["OutlinePen"]}");
                        xmlResult += Xml_helper.AddLabel("Style", false);
                    }
                }
                xmlResult += Xml_helper.AddLabel("Layer", false);
            }

            xmlResult += Xml_helper.AddLabel("Layer", false);

            // annadiendo informacion de los tematicos al xml respuesta
            xmlResult += Xml_helper.AddLabel("Tematic_data", true, false, new Tuple<string, string>("queryable", "true"), new Tuple<string, string>("opaque", "false"), new Tuple<string, string>("noSubsets", "true"), new Tuple<string, string>("fixedWidth", "0"), new Tuple<string, string>("fixedHeight", "0"));

            foreach (var tematic in tematics)
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
            xmlResult += Xml_helper.AddLabel("Tematic_data", false);



            xmlResult += Xml_helper.AddLabel("Capability", false);
            xmlResult += Xml_helper.AddLabel("WMS_Capabilities", false);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlResult);

            //code below is used to correct xml indent
            StringWriter sw = new StringWriter();
            doc.Save(sw);
            string xml = sw.ToString();

            return new StringContent(xml, Encoding.UTF8, "text/xml");
        }

        private List<IWMSFunction> getTypeOfRequests()
        {
            List<IWMSFunction> result = new List<IWMSFunction>();
            var interfaceAssembly = Assembly.GetAssembly(typeof(IWMSFunction));
            foreach (var defType in interfaceAssembly.DefinedTypes)
            {
                if (defType.ImplementedInterfaces.Contains(typeof(IWMSFunction)))
                    result.Add((IWMSFunction)interfaceAssembly.CreateInstance(defType.ToString()));
            }
            return result;
        }

        public bool HasImage()
        {
            return false;
        }
    }
}