using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class LinearRing : LineString
    {
        public LinearRing(IList<Point> points) : base(points)
        {
            GeometryType = GeometryType.LinearRing;
            if (points.Count < 2)
                throw new InvalidOperationException("A LinearRing must have two points or more");
            if (points[0] != points[NumPoints - 1])
            {
                this.points.Add(points[0]);
                //throw new InvalidOperationException("The first and last points in a LinearRing must be the same");
            }
        }

        public override Geometry Intersection(Geometry geo)
        {
            throw new NotImplementedException();
        }

    }
}
