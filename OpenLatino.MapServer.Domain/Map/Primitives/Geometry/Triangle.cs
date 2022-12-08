using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class Triangle : Polygon
    {
        public Triangle(Point p1, Point p2, Point p3) : base(new LinearRing(new Point[] { p1, p2, p3 }))
        {
            //comprobando colinealidad de los puntos
            double a = p2.X - p1.X;
            double b = p2.Y - p1.Y;
            double A = -b;
            double B = a;
            double C = b * p1.X - a * p1.Y;

            if (A * p3.X + B * p3.Y + C == 0)
                throw new InvalidOperationException("The points do not make a valid triangule because the points are collinear");
        }
    }
}
