using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBush;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class LineString : Curve
    {
        public LineString(IList<Point> points) : base(points)
        {
            GeometryType = GeometryType.LineString;
        }

        public override double Length
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int NumPoints { get { return points.Count; } }

        public Point this[int N] { get { return points[N]; } }

        public override string ToWKTString()
        {
            bool hasZ = points.First().Z.HasValue;
            bool hasM = points.First().M.HasValue;

            var result = "LINESTRING";
            if (hasZ)
                result += " z";
            if (hasM)
                result += "m";

            result += " (";

            bool firstAdded = false;
            foreach (var item in points.Select(p => p.ToWKTString().Replace("POINT", "")
                                                                 .Replace("z", "")
                                                                 .Replace("m", "")
                                                                 .TrimStart()
                                                                 .Replace("(", "")
                                                                 .Replace(")", "")))
            {
                result += firstAdded ? $", {item}" : item;
                firstAdded = true;
            }

            return $"{result})";
        }

        public override bool Intersects(Geometry geo)
        {
            bool insidepoint = false;
            bool outsidepoints = false;
            if (geo is Point)
            {
                if (!Contains(geo))
                {
                    //los puntos de la frontera interceptan
                    return UtilsGeometry.IsInPolygon((Point)geo, Points);
                }

                return insidepoint && outsidepoints;
            }
            else if (geo is Polygon)
            {
                return Intersects((geo as Polygon).ExteriorRing);
            }
            //Este caso encierra a los linear ring que heredan de linestring y por tanto
            // la resolucion del problema para los poligonos.
            else if (geo is LineString)
            {
                foreach (var point in (geo as LineString).Points)
                {
                    if (UtilsGeometry.IsInPolygon(point, Points))
                        insidepoint = true;
                    else
                        outsidepoints = true;
                }

                var temp = geo as LineString;
                bool insidepoint1 = false;
                bool outsidepoints1 = false;
                foreach (var point in Points)
                {
                    if (UtilsGeometry.IsInPolygon(point, temp.Points))
                        insidepoint1 = true;
                    else
                        outsidepoints1 = true;
                }
                return (insidepoint && outsidepoints)||(insidepoint1 && outsidepoints1);
            }
            throw new NotImplementedException();
        }

        public override bool Contains(Geometry geo)
        {
            if (geo is Point)
            {
                return UtilsGeometry.IsPointInsidePolygon((Point)geo, Points.ToList());
            }
            else if (geo is Polygon)
            {
                // si es una linea recta no puede contener un poligono
                if (Points.Count() == 2)
                    return false;
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

        public override Geometry Intersection(Geometry geo)
        {

            throw new NotImplementedException();
        }


        public override Envelope Envelope()
        {
            return UtilsGeometry.BoungingBox(Points.ToList());
        }

    }
}
