using Newtonsoft.Json;
using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenLatino.MapServer.Infrastructure.SQL.DataSource
{
    public class SQLWMSCapabilitiesQueryFuncs : IWMSCapabilitiesQuery
    {
        public Type Provider { get; set; } = typeof(DBContextProvider);

        public string Service { get; set; }
        public string splitOn { get; set; }
        public Type[] TypesReturn { get; set; }
        public string Version { get; set; }
        public string InfoColums { get; set; }
        public string ProviderGeoField { get; set; }
        public string ProviderPkField { get; set; }
        public Tuple<Point, Point> BoundingBox { get; set; }

        public object ConverterFunction(object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetGeometries<T>(IFileDataStructure<T> datastruct) where T : IFeature
        {
            throw new NotImplementedException();
        }

        public object GetQuery()
        {
            string cwd = Directory.GetCurrentDirectory();
            string db;
            dynamic jsn = JsonConvert.DeserializeObject(File.ReadAllText($"{cwd}\\appsettings.json"));
            try { db = jsn["AdminDbName"]; }
            catch { db = "Admin_Database_renew"; }

            var q = $@"SELECT [Id]
                     ,[Name]
                     FROM [{db}].[dbo].[ServiceFunctions]";

            return q;
        }
    }
}
