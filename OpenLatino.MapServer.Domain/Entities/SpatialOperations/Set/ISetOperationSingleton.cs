using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.FileData;
using System.Collections.Generic;

namespace OpenLatino.MapServer.Domain.Entities.SpatialOperation.Set
{
    public interface ISetOperationSingleton
    {
        IEnumerable<GeometryWithFeatures> ResolveSetOperation(SetOperetionType operationType, IEnumerable<GeometryWithFeatures> a, IEnumerable<GeometryWithFeatures> b);
    }
}