//#define CHECK_CONSTRAINT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBush;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class GeometryCollection : Geometry
    {
        protected IList<Geometry> list = new List<Geometry>();

        public GeometryCollection(IEnumerable<Geometry> collection) : base()
        {
            list = new List<Geometry>(collection);
#if CHECK_CONSTRAINT
            var srid = list[0].SRID;
            if (list.Any(geo => geo.SRID != srid))
                throw new InvalidOperationException("Not is possible to have a collection with different spatial reference system");
#endif
            this.GeometryType = GeometryType.GeometryCollection;
        }

        public int NumGeometries { get { return list.Count; } }

        public Geometry this[int n] { get { return list[n]; } }

        public override Envelope Envelope()
        {
            throw new NotImplementedException();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }

        public override Geometry Boundary()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Disjoint(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Intersects(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Touches(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Crosses(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Within(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Overlaps(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Relate(Geometry geo, string matrix)
        {
            throw new NotImplementedException();
        }

        public override Geometry LocatoAlong(double mValue)
        {
            throw new NotImplementedException();
        }

        public override Geometry LocateBetween(double mStart, double mEnd)
        {
            throw new NotImplementedException();
        }

        public override double Distance(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry Buffer(double distance)
        {
            throw new NotImplementedException();
        }

        public override Geometry ConvexHull()
        {
            throw new NotImplementedException();
        }

        public override Geometry Intersection(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry Union(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry Difference(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry SymDifference(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override string ToWKTString()
        {
            throw new NotImplementedException();
        }
    }
}
