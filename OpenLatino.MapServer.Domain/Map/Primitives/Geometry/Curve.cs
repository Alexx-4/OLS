using RBush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public abstract class Curve : Geometry
    {
        protected IList<Point> points;

        public Curve(IList<Point> points) : base()
        {
            this.points = points;
            Dimension = 1;
        }

        public abstract double Length { get; set; }

        public Point StartPoint { get { return points[0]; } }

        public Point EndPoint { get { return points[points.Count - 1]; } }

        internal IEnumerable<Point> Points { get { return points; } }

        public bool IsClosed { get { return StartPoint == EndPoint; } set { } }

        public bool IsRing { get { return IsSimple && IsClosed; } set { } }

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
