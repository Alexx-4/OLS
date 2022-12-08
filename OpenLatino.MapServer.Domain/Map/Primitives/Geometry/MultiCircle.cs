using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class MultiCircle : MultiSurface
    {
        public MultiCircle(IEnumerable<Circle> circles) : base(circles)
        {

        }

        public override double Area()
        {
            throw new NotImplementedException();
        }

        public override Point Centroid()
        {
            throw new NotImplementedException();
        }

        public override Point PointOnSurface()
        {
            throw new NotImplementedException();
        }
    }
}
