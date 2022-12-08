using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class MultiPolygon : MultiSurface
    {
        public MultiPolygon(IEnumerable<Polygon> polygons) : base(polygons)
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

        public override string ToWKTString()
        {
            bool hasZ = ((Polygon)list)[0].Points.First().Z.HasValue;
            bool hasM = ((Polygon)list)[0].Points.First().M.HasValue;
            var result = "MULTIPOLYGON";

            if (hasZ)
                result += " z";
            if (hasM)
                result += " m";

            result += " (";

            bool firstAdded = false;
            foreach (var item in list.Select(p => p.ToWKTString().Replace("POLYGON", "").Replace("z", "").Replace("m", "").TrimStart()))
            {
                result += firstAdded ? $", {item}" : item;
                firstAdded = true;
            }

            return $"{result})";
        }
    }
}
