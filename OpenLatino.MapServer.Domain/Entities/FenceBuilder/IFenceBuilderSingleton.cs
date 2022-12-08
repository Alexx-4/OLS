using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.FenceBuilder
{
    public interface IFenceBuilderSingleton
    {
        /// <summary>
        /// CDetermina si se puede crear una geometria a partir de un string yonstruye la geometria
        /// </summary>
        /// <param name="fence"></param>
        /// <returns></returns>
        Geometry Build(BorderShape fenceShape, string fence);
    }
}