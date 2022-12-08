using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using RBush;

namespace OpenLatino.MapServer.Domain.Entities.FileData
{
    /// <summary>
    /// A Base interface for spatial data loaded
    /// </summary>
    public interface IFeature : ISpatialData
    {
        /// <summary>
        /// Identifier of this feature
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// The geometry of this feature
        /// </summary>
        Geometry Geometry { get; set; }


    }
}