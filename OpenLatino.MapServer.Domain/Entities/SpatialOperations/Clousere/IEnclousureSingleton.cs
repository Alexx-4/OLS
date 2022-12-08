using OpenLatino.MapServer.Domain.Entities.FileData;
using System.Collections.Generic;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using RBush;

namespace OpenLatino.MapServer.Domain.Entities.SpatialOperation.Clousere
{
    public interface IEnclousureSingleton
    {
        IEnumerable<GeometryWithFeatures> ResolveClousere(EnclousureType enclosure, Geometry fence, Envelope bbox);
        IEnumerable<GeometryWithFeatures> ResolveClousere<T>(EnclousureType enclosure, Geometry fence, IFileDataStructure<T> datastruct, Envelope bbox) where T : IFeature;
    }
}