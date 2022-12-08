using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBush;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class Polygon : Surface, IEnumerable<LinearRing>
    {
        public Polygon(LinearRing exteriorRing) : this(exteriorRing, new LinearRing[0])
        {

        }

        public Polygon(LinearRing exteriorRing, IEnumerable<LinearRing> interiorRings) : base()
        {
            ExteriorRing = exteriorRing;
            InteriorRings = new List<LinearRing>(interiorRings);
            GeometryType = GeometryType.Polygon;
        }

        public IList<LinearRing> InteriorRings { get; private set; }

        public LinearRing ExteriorRing { get; private set; }

        public int NumInteriorRing { get { return InteriorRings.Count(); } }

        public LinearRing this[int N] { get { return InteriorRings[N]; } }

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

        public override Envelope Envelope()
        {
            return UtilsGeometry.BoungingBox(this.ExteriorRing.Points.ToList());
        }

        public override string ToWKTString()
        {
            bool hasZ = ExteriorRing.Points.First().Z.HasValue;
            bool hasM = ExteriorRing.Points.First().M.HasValue;

            string result = "POLYGON";

            if (hasZ)
                result += " z";
            if (hasM)
                result += "m";

            result += $" ({ ExteriorRing.ToWKTString().Replace("LINESTRING", "").Replace("z", "").Replace("m", "").TrimStart()}";

            foreach (var item in InteriorRings)
                result += $" {item.ToWKTString().Replace("LINESTRING", "").Replace("z", "").Replace("m", "").TrimStart()}";


            return $"{result})";
        }

        public IEnumerator<LinearRing> GetEnumerator()
        {
            for (int i = 0; i < NumInteriorRing; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Intersects(Geometry geo)
        {
            return ExteriorRing.Intersects(geo);
        }

        public override bool Contains(Geometry geo)
        {
            return ExteriorRing.Contains(geo);
        }
    }
}
