using RBush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class Circle : Surface
    {
        public Circle() : this(new Point(), 1)
        {
        }

        public Circle(Point center, double ratio) : base()
        {
            if (ratio < 0)
                throw new InvalidOperationException("No se puede instanciar un círculo con radio negativo");

            Center = center;
            Ratio = ratio;

            GeometryType = GeometryType.Circle;
        }

        public Point Center { get; set; }
        public double Ratio { get; set; }

        public override double Area()
        {
            throw new NotImplementedException();
        }

        public override Point Centroid()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(Geometry geo)
        {
            if (geo is Point)
            {
                return UtilsGeometry.IsPointInsideCircle((Point)geo, Center, Ratio);
            }
            else if (geo is Polygon)
            {
                return Contains((geo as Polygon).ExteriorRing);
            }
            //Este caso encierra a los linear ring que heredan de linestring
            else if (geo is LineString)
            {
                foreach (var point in (geo as LineString).Points)
                {
                    if (!Contains(point))
                        return false;
                }
                return true;
            }

            throw new NotImplementedException();
        }

        public override bool Intersects(Geometry geo)
        {
            bool insidepoint = false;
            bool outsidepoints = false;
            if (geo is Point)
            {
                return UtilsGeometry.IsPointOverCircle((Point)geo, Center, Ratio);
            }
            else if (geo is Polygon)
            {
                return Intersects((geo as Polygon).ExteriorRing);
            }
            //Este caso encierra a los linear ring que heredan de linestring
            else if (geo is LineString)
            {
                foreach (var point in (geo as LineString).Points)
                {
                    if (UtilsGeometry.IsPointInsideCircle(point, Center, Ratio) )
                        insidepoint = true;
                    else
                        outsidepoints = true;

                }
                return insidepoint && outsidepoints;
            }
            throw new NotImplementedException();
        }

        public override Point PointOnSurface()
        {
            throw new NotImplementedException();
        }

        public override Envelope Envelope()
        {
            return UtilsGeometry.BoundingBoxCircle(this);
        }

        public override string ToWKTString()
        {
            return $"CIRCLE ({Ratio} {Center.ToWKTString().Replace("POINT", "").Replace("(", "").Replace(")", "").TrimStart()})";
        }
    }
}
