using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.FileData;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace OpenLatino.MapServer.Domain.Entities.SpatialOperation.Set
{
    public static class SetOperation
    {
        public static List<Geometry> RessolveOperation(SetOperetionType operation, List<IFeature> a, List<IFeature> b)
        {
            if (operation == SetOperetionType.Union)
                return Union(a, b);
            else if (operation == SetOperetionType.INTERSECT)
                return Intersection(a, b);
            else
                throw new NotImplementedException("Operacion de conjunto no implementada");
        }

        public static List<Geometry> Union(List<IFeature> a, List<IFeature> b)
        {
            return a.Union(b).Select(x => x.Geometry).ToList();
        }
        public static List<Geometry> Intersection(List<IFeature> a, List<IFeature> b)
        {
            return a.Intersect(b).Select(x => x.Geometry).ToList();
        }

    }
}
