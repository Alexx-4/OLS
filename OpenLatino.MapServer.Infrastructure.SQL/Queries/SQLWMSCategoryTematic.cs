using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Infrastructure.SQL.DataSource;
using System;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Infrastructure.SQL.Queries
{
    public class SQLWMSCategoryTematic : IWMSQuery
    {
        public Type Provider { get; set; } = typeof(DBContextProvider);
        public Type[] TypesReturn { get; set; } = { typeof(object) };
        public string splitOn { get; set; } = "Id";
        public string InfoColums { get; set; }
        public string ProviderGeoField { get; set ; }
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
            return $"SELECT DISTINCT {ProviderPkField} FROM {ProviderGeoField} WHERE {ProviderPkField} IS NOT NULL";
        }
    }
}
