using System.Collections.Generic;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.SpatialOperation.Set
{
    public interface ISetOperator
    {
        bool CanOperate(SetOperetionType setoperation);

        List<IFeature> ResolveSetOperation(IEnumerable<IFeature> a, IEnumerable<IFeature> b);

    }
}