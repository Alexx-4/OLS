using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBush;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public abstract class Surface : Geometry
    {
        public Surface() : base()
        {
            Dimension = 2;
        }

        public abstract double Area();

        public abstract Point Centroid();

        public abstract Point PointOnSurface();

        public override Geometry Boundary()
        {
            throw new NotImplementedException();
        }

        public override Geometry Buffer(double distance)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry ConvexHull()
        {
            throw new NotImplementedException();
        }

        public override bool Crosses(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry Difference(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Disjoint(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override double Distance(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Envelope Envelope()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry Intersection(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Intersects(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry LocateBetween(double mStart, double mEnd)
        {
            throw new NotImplementedException();
        }

        public override Geometry LocatoAlong(double mValue)
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

        public override Geometry SymDifference(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }

        public override bool Touches(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override Geometry Union(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Within(Geometry geo)
        {
            throw new NotImplementedException();
        }
    }
}
