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
    public class SQLWMSCapabilitiesQuery : IWMSCapabilitiesQuery
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

            var q = $@"SELECT  [{db}].[dbo].[AlfaInfos].[LayerID] as LayerName
                 ,ls.[Id] as StyleName
                 ,ls.[Name] as StyleTitle
                 ,[{db}].[dbo].[AlfaInfos].[Table] as LayerTitle
                    ,[EnableOutline]
                    ,[Fill]
                    ,[Line]
                    ,[OutlinePen]
                    FROM (SELECT * FROM [{db}].[dbo].[Styles] inner join [{db}].[dbo].[LayerStyles] ON [{db}].[dbo].[Styles].Id = [{db}].[dbo].[LayerStyles].VectorStyleId) AS ls
                 Inner Join [{db}].[dbo].AlfaInfos
                    ON [{db}].[dbo].AlfaInfos.LayerID = ls.LayerId";

            return q;
        }       
    }
}
