using RBush;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    /// <summary>
    /// Class for common method useds in geometries
    /// </summary>
    public static class UtilsGeometry
    {
        /// <summary>
        /// Determine if a point is inside a polygon or in the border
        /// </summary>
        /// <param name="point"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool IsInPolygon(this Point point, IEnumerable<Point> polygon)
        {
            bool result = false;
            var a = polygon.Last();
            foreach (var b in polygon)
            {
                if ((b.X == point.X) && (b.Y == point.Y))
                    return true;

                if ((b.Y == a.Y) && (point.Y == a.Y) && (a.X <= point.X) && (point.X <= b.X))
                    return true;

                if ((b.Y < point.Y) && (a.Y >= point.Y) || (a.Y < point.Y) && (b.Y >= point.Y))
                {
                    if (b.X + (point.Y - b.Y) / (a.Y - b.Y) * (a.X - b.X) <= point.X)
                        result = !result;
                }
                a = b;
            }
            return result;
        }


        /// <summary>
        /// Determines if the given point is inside the polygon
        /// </summary>
        /// <param name="polygon">the vertices of polygon</param>
        /// <param name="testPoint">the given point</param>
        /// <returns>true if the point is inside the polygon; otherwise, false</returns>
        public static bool IsPointInsidePolygon(Point testPoint, List<Point> polygon)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        /// <summary>
        /// Get The Bounding Box for a list of points
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static Envelope BoungingBox(List<Point> polygon)
        {
            double maxX = polygon.Max(p => p.X);
            double maxY = polygon.Max(p => p.Y);
            double minX = polygon.Min(p => p.X);
            double minY = polygon.Min(p => p.Y);

            return new RBush.Envelope(minX, minY, maxX, maxY);
        }

        public static Envelope BoundingBoxCircle(Circle circle)
        {
            var x = circle.Center.X;
            var y = circle.Center.Y;
            var ratio = circle.Ratio;

            var upLeftCorner = new Point(x-ratio,y+ratio);
            var upRightCorner = new Point(x+ratio,y+ratio);
            var lowRigthCorner = new Point(x+ratio,y-ratio);
            var lowLeftCorner = new Point(x-ratio,y-ratio);

            return BoungingBox(new List<Point>() {upLeftCorner, upRightCorner, lowRigthCorner, lowLeftCorner});

        }

        /// <summary>
        /// Determine if the given point is inside the circle
        /// </summary>
        /// <param name="testPpooint"></param>
        /// <param name="center"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public static bool IsPointInsideCircle(Point testPoint, Point center, double ratio)
        {
            return Math.Pow((testPoint.X - center.X), 2) + Math.Pow((testPoint.Y - center.Y), 2) <= Math.Pow(ratio, 2);
        }


        /// <summary>
        /// Determine if the given point is over the circle
        /// </summary>
        /// <param name="testPpooint"></param>
        /// <param name="center"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public static bool IsPointOverCircle(Point testPoint, Point center, double ratio, double epsilon = 0.00005)
        {
            //Garantiza que este dento de la circunferencia
            bool p1 = Math.Pow((testPoint.X - center.X), 2) + Math.Pow((testPoint.Y - center.Y), 2) <=
                      Math.Pow(ratio, 2) + epsilon;
            //Garantiza que este sobre el borde solamente
            bool p2 = Math.Pow((testPoint.X - center.X), 2) + Math.Pow((testPoint.Y - center.Y), 2) >=
                      Math.Pow(ratio, 2) - epsilon;
            return p1 && p2;
        }

    }
}