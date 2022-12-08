using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.MapServer.Domain.Entities.Querys;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Infrastructure.SQL.DataSource;
using System;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Infrastructure.SQL.Queries
{
    public class SQLWMSAlphaInfoQuery : IWMSQuery
    {
        public Type Provider { get; set; } = typeof(DBContextProvider);
        public Type[] TypesReturn { get; set; } = { typeof(object) };
        public string splitOn { get; set; } = "Id";
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
            string query = "SELECT CAST(table_name as varchar) as TableName FROM INFORMATION_SCHEMA.TABLES";

            if(InfoColums != null)
            {
                query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{InfoColums}'";
            }

            return query;
        }
    }
}
