using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public abstract class MultiSurface : GeometryCollection
    {
        public MultiSurface(IEnumerable<Surface> surfaces) : base(surfaces)
        {

        }

        public abstract double Area();

        public abstract Point Centroid();

        public abstract Point PointOnSurface();
    }
}
