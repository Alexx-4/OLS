using System.Collections.Generic;
using System.Linq;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.MapServer.Domain.Entities.SpatialOperation.Clousere;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using RBush;

namespace OpenLatino.MapServer.Infrastructure.ShpProv.DataSource.Enclousure
{
    //public sealed class ShapeEnclousureSingleton : IEnclousureSingleton
    //{
    //    private ShapeEnclousureSingleton()
    //    {
    //        _diccEnclousure = new Dictionary<EnclousureType, ResolveEnclousure<IFeature>>();
    //        Initialice();
    //    }

    //    public static ShapeEnclousureSingleton Instance { get { return Nested.instance; } }

    //    private class Nested
    //    {
    //        // Explicit static constructor to tell C# compiler
    //        // not to mark type as beforefieldinit
    //        static Nested()
    //        {
    //        }

    //        internal static readonly ShapeEnclousureSingleton instance = new ShapeEnclousureSingleton();
    //    }

    //    private delegate IEnumerable<GeometryWithFeatures> ResolveEnclousure<T>(Geometry fence,
    //            IFileDataStructure<T> datastruct, Envelope boundingBox) where T : IFeature;

    //    private Dictionary<EnclousureType, ResolveEnclousure<IFeature>> _diccEnclousure;


    //    public IEnumerable<GeometryWithFeatures> ResolveClousere(EnclousureType enclosure, Geometry fence)
    //    {
    //        throw new System.NotImplementedException();
    //    }


    //    private void Initialice()
    //    {
    //        _diccEnclousure.Add(EnclousureType.In, In);
    //        _diccEnclousure.Add(EnclousureType.Over, Over);
    //        _diccEnclousure.Add(EnclousureType.InOver, InOver);
    //        _diccEnclousure.Add(EnclousureType.Out, Out);
    //        _diccEnclousure.Add(EnclousureType.OutOver, OutOver);
    //    }

    //    IEnumerable<GeometryWithFeatures> IEnclousureSingleton.ResolveClousere<T>(EnclousureType enclosure, Geometry fence, IFileDataStructure<T> datastruct)
    //    {
    //        if (_diccEnclousure.ContainsKey(enclosure))
    //            return _diccEnclousure[enclosure](fence, datastruct as IFileDataStructure<IFeature>, fence.Envelope());
    //        return null;
    //    }


    //    // Operaciones de clausura

    //    private IEnumerable<IFeature> In(Geometry fence, IFileDataStructure<IFeature> datastruct, Envelope boundingBox)
    //    {
    //        var result = new List<IFeature>();
    //        var candidates = datastruct.Search(boundingBox);
    //        foreach (var feature in candidates)
    //        {
    //            if (fence.Contains(feature.Geometry))
    //                result.Add(feature);

    //        }
    //        return result;
    //    }

    //    private IEnumerable<IFeature> Over(Geometry fence, IFileDataStructure<IFeature> datastruct, Envelope boundingBox)
    //    {
    //        var result = new List<IFeature>();
    //        var candidates = datastruct.Search(boundingBox);
    //        foreach (var feature in candidates)
    //        {
    //            if (fence.Intersects(feature.Geometry))
    //                result.Add(feature);

    //        }
    //        return result;

    //    }
    //    private IEnumerable<IFeature> InOver(Geometry fence, IFileDataStructure<IFeature> datastruct, Envelope boundingBox)
    //    {
    //        var inside = new List<IFeature>();
    //        var over = new List<IFeature>();
    //        var candidates = datastruct.Search(boundingBox);
    //        foreach (var feature in candidates)
    //        {
    //            if (fence.Contains(feature.Geometry))
    //                inside.Add(feature);
    //            if (fence.Intersects(feature.Geometry))
    //                over.Add(feature);

    //        }
    //        return inside.Union(over);
    //    }

    //    private IEnumerable<IFeature> Out(Geometry fence, IFileDataStructure<IFeature> datastruct, Envelope boundingBox)
    //    {
    //        var result = new List<IFeature>();
    //        var candidates = datastruct.Search(boundingBox);
    //        foreach (var feature in candidates)
    //        {
    //            if (!fence.Contains(feature.Geometry) && !(fence.Intersects(feature.Geometry)))
    //                result.Add(feature);
    //        }
    //        return result;
    //    }

    //    private IEnumerable<IFeature> OutOver(Geometry fence, IFileDataStructure<IFeature> datastruct, Envelope boundingBox)
    //    {
    //        var outside = new List<IFeature>();
    //        var over = new List<IFeature>();
    //        var candidates = datastruct.Search(boundingBox);
    //        foreach (var feature in candidates)
    //        {
    //            if (!fence.Contains(feature.Geometry))
    //                outside.Add(feature);
    //            if (fence.Intersects(feature.Geometry))
    //                over.Add(feature);

    //        }
    //        return outside.Union(over);
    //    }

    //}
}