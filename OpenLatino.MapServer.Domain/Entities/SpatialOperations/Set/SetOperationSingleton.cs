using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.FileData;

using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.SpatialOperation.Set
{
    //public class SetOperationSingleton : ISetOperationSingleton
    //{

    //    private Dictionary<SetOperetionType, SetOperation> _diccOperation;
    //    private SetOperationSingleton()
    //    {
    //        _diccOperation = new Dictionary<SetOperetionType, SetOperation>();
    //        Initialice();
    //    }

    //    public static SetOperationSingleton Instance { get { return Nested.instance; } }

    //    private class Nested
    //    {
    //        // Explicit static constructor to tell C# compiler
    //        // not to mark type as beforefieldinit
    //        static Nested()
    //        {
    //        }

    //        internal static readonly SetOperationSingleton instance = new SetOperationSingleton();
    //    }

    //    private delegate IEnumerable<IFeature> SetOperation(IEnumerable<IFeature> a, IEnumerable<IFeature> b);
    //    public IEnumerable<IFeature> ResolveSetOperation(SetOperetionType operationType, IEnumerable<IFeature> a, IEnumerable<IFeature> b)
    //    {
    //        if (_diccOperation.ContainsKey(operationType))
    //            return _diccOperation[operationType](a, b);
    //        return null;
    //    }

    //    private IEnumerable<IFeature> Union(IEnumerable<IFeature> a, IEnumerable<IFeature> b)
    //    {
    //        return a.Union(b).ToList();
    //    }
    //    private IEnumerable<IFeature> Intersection(IEnumerable<IFeature> a, IEnumerable<IFeature> b)
    //    {
    //        return a.Intersect(b).ToList();
    //    }

    //    private IEnumerable<IFeature> Difference(IEnumerable<IFeature> a, IEnumerable<IFeature> b)
    //    {
    //        return a.Except(b).ToList();
    //    }

    //    private void Initialice()
    //    {
    //        _diccOperation[SetOperetionType.Union] = Union;
    //        _diccOperation[SetOperetionType.Intersection] = Intersection;
    //        _diccOperation[SetOperetionType.Difference] = Difference;

    //    }

    //}
}