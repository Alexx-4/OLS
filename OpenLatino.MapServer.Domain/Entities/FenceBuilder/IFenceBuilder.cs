using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.FenceBuilder
{
    /// <summary>
    /// Clase base para poder construir geometrias a partir de strings
    /// </summary>
    public interface IFenceBuilder
    {
        /// <summary>
        /// Determina si se puede crear una geometria a partir de un string
        /// </summary>
        /// <param name="fenceFormat"></param>
        /// <returns></returns>
        bool CanBuild(BorderShape fenceShape);

        /// <summary>
        /// Construye una geometria a partir de un string
        /// </summary>
        /// <param name="fence"></param>
        /// <returns></returns>
        Geometry Build(string fence);
    }
}