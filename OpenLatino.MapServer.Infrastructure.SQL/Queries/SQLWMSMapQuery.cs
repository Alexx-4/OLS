using OpenLatino.MapServer.Domain.Entities;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.MapServer.Infrastucture.SQL.DataSource
{
    public class SQLWMSMapQuery : IWMSMapQuery
    {
        public SQLWMSMapQuery()
        {


        }
        public Tuple<Point, Point> BoundingBox { get; set; }
        public string Crs { get; set; }

        public Type Provider { get; set; } = typeof(ProviderSQL);


        public string ProviderGeoField { get; set; }

        public string ProviderPkField { get; set; }

        public string ProviderTable { get; set; }

        public string splitOn { get; set; } = "Id";

        public string InfoColums { get; set; }

        public Type[] TypesReturn { get; set; } = { typeof(object) };

        public object ConverterFunction(object[] paramaters)
        {
            return paramaters;
        }

        public IEnumerable<object> GetGeometries<T>(IFileDataStructure<T> datastruct) where T : IFeature
        {
            throw new NotImplementedException();
        }

        public object GetQuery()
        {
            string minx = BoundingBox.Item1.X.ToString(), miny = BoundingBox.Item1.Y.ToString(), maxx = BoundingBox.Item2.X.ToString(), maxy = BoundingBox.Item2.Y.ToString();
            var crs = Crs;
            return $@"
                    DECLARE @g Geometry
                    SET @g=Geometry::STGeomFromText('POLYGON(({minx} {miny}, {minx} {maxy}, {maxx} {maxy}, {maxx} {miny}, {minx} {miny}))', {crs})
                    SELECT {ProviderPkField},@g.STIntersection({ProviderGeoField}) as ogr_geometry,{InfoColums} FROM {ProviderTable}
                    WHERE {ProviderGeoField}.STIntersects(@g) = 1";
        }
    }
}
