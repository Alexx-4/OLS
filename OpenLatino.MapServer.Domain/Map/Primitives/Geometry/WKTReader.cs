using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class WKTReader
    {
        private IEnumerable<Tuple<string, Func<string, Geometry>>> geometryConverters = new Tuple<string, Func<string, Geometry>>[]
        {
            new Tuple<string, Func<string, Geometry>>("POINT",GetPoint),
            new Tuple<string, Func<string, Geometry>>("LINESTRING",GetLineString),
            new Tuple<string, Func<string, Geometry>>("MULTILINESTRING",GetMultiLineString),
            new Tuple<string, Func<string, Geometry>>("POLYGON",GetPolygon),
            new Tuple<string, Func<string, Geometry>>("MULTIPOLYGON",GetMultiPolygon),
            new Tuple<string, Func<string, Geometry>>("CIRCLE",GetCircle),
            new Tuple<string, Func<string, Geometry>>("MULTICIRCLE",GetMultiCircle),
        };



        public Geometry GetGeometry(string geoText)
        {
            foreach (var item in geometryConverters)
                if (geoText.StartsWith(item.Item1))
                    return item.Item2(geoText);

            throw new InvalidOperationException($"The input does not define a valid geometry or the reader does not implement a converter for the geometry entried. The input was: {geoText}");
        }

        private static Geometry GetPoint(string pointText)
        {
            double[] coord = pointText.Replace("POINT", "")
                                      .TrimStart()
                                      .Replace("(", "")
                                      .Replace(")", "")
                                      .Split(' ')
                                      .Select(v => double.Parse(v))
                                      .ToArray();

            var result = new Point(coord[0], coord[1]);
            if (coord.Length > 2)
                result.Z = coord[2];
            if (coord.Length > 3)
                result.M = coord[3];

            return result;
        }

        private static Geometry GetLineString(string lineStringText)
        {
            Point[] points = lineStringText.Replace("LINESTRING", "")
                                           .TrimStart()
                                           .Replace("(", "")
                                           .Replace(")", "")
                                           .Split(',')
                                           .Select(i => i.TrimStart().Split(' '))
                                           .Select(p => new Point(double.Parse(p[0]), double.Parse(p[1])))
                                           .ToArray();

            return new LineString(points);
        }

        private static Geometry GetMultiLineString(string multiLineStringText)
        {
            var t = multiLineStringText.Replace("MULTILINESTRING", "").TrimStart().Remove(0, 1);
            t = t.Remove(t.Length - 1, 1);
            var lineStrings = new List<LineString>();

            for (int i = 0; i < t.Length; i++)
            {
                int j = t.IndexOf(')', i);
                lineStrings.Add(GetLineString($"LINESTRING {t.Substring(i, j - i + 1)}") as LineString);
                i = j + 2;
            }

            return new MultiLineString(lineStrings);
        }

        private static Geometry GetLineRing(string lineRing)
        {
            Point[] points = lineRing.Replace("LINEARRING", "")
                                     .TrimStart()
                                     .Replace("(", "")
                                     .Replace(")", "")
                                     .Split(',')
                                     .Select(i => i.TrimStart().Split(' '))
                                     .Select(p => new Point(double.Parse(p[0]), double.Parse(p[1])))
                                     .ToArray();

            return new LinearRing(points);
        }

        private static Geometry GetPolygon(string polygonText)
        {
            var t = polygonText.Replace("POLYGON", "").TrimStart().Remove(0, 1);
            t = t.Remove(t.Length - 1, 1);
            var rings = new List<LinearRing>();

            for (int i = 0; i < t.Length; i++)
            {
                int j = t.IndexOf(')', i);
                rings.Add(GetLineRing($"LINEARRING {t.Substring(i, j - i + 1)}") as LinearRing);
                i = j + 2;
            }

            return new Polygon(rings[0], rings.Skip(1));
        }

        private static Geometry GetMultiPolygon(string multiPolygonText)
        {
            var t = multiPolygonText.Replace("MULTIPOLYGON", "").TrimStart().Remove(0, 1);
            t = t.Remove(t.Length - 1, 1);
            var polygons = new List<Polygon>();

            for (int i = 0; i < t.Length; i++)
            {
                int count = 0;
                int j;
                for (j = i + 1; j < t.Length; j++)
                {
                    if (t[j] == '(') count += 1;
                    else if (t[j] == ')') count -= 1;

                    if (count == -1)
                        break;
                }

                polygons.Add(GetPolygon($"POLYGON {t.Substring(i, j - i + 1)}") as Polygon);
                i = j + 2;
            }

            return new MultiPolygon(polygons);
        }

        private static Geometry GetMultiCircle(string geoText)
        {
            var collection = geoText.Replace("MULTICIRCLE", "").TrimStart().Remove(0, 1);
            collection = collection.Remove(collection.Length - 1, 1);
            var circles = new List<Circle>();

            for (int i = 0; i < collection.Length; i++)
                if (collection[i] == '(')
                {
                    int j = collection.IndexOf(')', i);
                    circles.Add(GetCircle($"CIRCLE {collection.Substring(i, j - i + 1)}") as Circle); 
                }

            return new MultiCircle(circles);
        }

        private static Geometry GetCircle(string geoText)
        {
            var v = geoText.Replace("CIRCLE", "").Replace("(", "").Replace(")", "").TrimStart().Split(' ').Select(i => double.Parse(i)).ToArray();
            var ratio = v[0];
            var x = v[1];
            var y = v[2];

            double? z = null, m = null;
            if (v.Length > 3)
                z = v[3];

            if (v.Length > 4)
                z = v[4];

            return new Circle(new Point(x, y, z, m), ratio);
        }
    }
}
