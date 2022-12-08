using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.MapServer.Infrastucture.SQL.DataSource
{
    public class SQLWMSFeatureInfoQuery : IWMSFeatureInfoQuery
    {
        public string Crs { get; set; }

        public int Feature_Count { get; set; }

        public string InfoColums { get; set; }

        public Point Point { get; set; }

        public Type Provider { get; set; } = typeof(ProviderSQL);

        public string ProviderGeoField { get; set; }

        public string ProviderPkField { get; set; }

        public string ProviderTable { get; set; }

        public string splitOn { get; set; }

        public Type[] TypesReturn { get; set; }
        public Tuple<Point, Point> BoundingBox { get; set; }

        public object ConverterFunction(object[] parameters)
        {
            return parameters.Select(Convert.ToString).ToArray();
        }

        public IEnumerable<object> GetGeometries<T>(IFileDataStructure<T> datastruct) where T : IFeature
        {
            throw new NotImplementedException();
        }

        public object GetQuery()
        {
            //var a = Feature_Count;
            var b = $@"
                    DECLARE @g Geometry
                    SET @g=Geometry::STGeomFromText('POINT({Point.X} {Point.Y})', {Crs})
                    SELECT TOP {Feature_Count.ToString()} {ProviderPkField}, {ProviderGeoField}, {InfoColums} FROM {ProviderTable}
                    WHERE {ProviderGeoField}.STDistance(@g) IS NOT NULL
                    ORDER BY {ProviderGeoField}.STDistance(@g)";
            return b;
        }
    }
}
