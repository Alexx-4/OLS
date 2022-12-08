using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using RBush;
using OpenLatino.Core.Domain.Entities;

namespace OpenLatino.MapServer.Domain.Entities.Querys
{
    public interface IQuery
    {
        Type Provider { get; set; }

        object GetQuery();

        object ConverterFunction(object[] parameters);

        Type[] TypesReturn { get; set; }

        string splitOn { get; set; }

        string InfoColums { get; set; }
         string ProviderGeoField { get; set; }

        string ProviderPkField { get; set; }

        Tuple<Point, Point> BoundingBox { get; set; }

        IEnumerable<object> GetGeometries<T>(IFileDataStructure<T> datastruct) where T : IFeature;

     
    }
}
