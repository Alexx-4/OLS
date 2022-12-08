using OpenLatino.MapServer.Domain.Entities;
using System;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.FileData;
using System.Collections.Generic;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Infrastructure.SQL.DataSource;
using OpenLatino.Core.Domain.Entities;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace OpenLatino.MapServer.Infrastucture.SQL.DataSource
{
    public class SQLWMSTematicCapabilitiesQuery : IWMSCapabilitiesQuery
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

            //var db = "Admin_Database_renew"; //Cada vez que cambie la base de datos hay que cambiar esto

            var q = $@"SELECT    [Id] as TematicId
                                ,[Name] as TematicName
                       FROM [{db}].[dbo].[TematicLayers]";

            return q;
        }
    }
}
