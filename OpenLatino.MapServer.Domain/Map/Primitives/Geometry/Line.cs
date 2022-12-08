using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class Line : LineString
    {
        public Line(Point p1, Point p2) : base(new List<Point>() { p1, p2 })
        {

        }

        public Point Point1 { get { return points[0]; } }

        public Point Point2 { get { return points[1]; } }
    }
}
