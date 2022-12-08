using RBush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class Point : Geometry
    {
        public Point() : this(0, 0)
        {
        }

        public Point(double x, double y, double? z = null, double? m = null) : base()
        {
            X = x;
            Y = y;
            Z = z;
            M = m;

            Is3D = Z.HasValue;
            IsMeasured = M.HasValue;
            IsSimple = true;
            CoordinateDimension = 2;
            Dimension = 0;
            GeometryType = GeometryType.Point;
        }

        public double? M { get; internal set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double? Z { get; internal set; }

        public override Geometry Boundary()
        {
            //Return an empty set
            throw new NotImplementedException();
        }

        public override Geometry Buffer(double distance)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(Geometry geo)
        {
            if (geo is Point)
                return this == ((Point)(geo));
            return geo.Contains(this);
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
            //Ambos son puntos
            if (geo.GeometryType == this.GeometryType)
                return this == (Point)geo;
            else
                return geo.Intersects(this);
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

        public override Envelope Envelope()
        {
            return UtilsGeometry.BoungingBox(new List<Point>() {this});
        }

        public override string ToWKTString()
        {
            if (Z.HasValue)
                return M.HasValue ? $"POINT zm ({X} {Y} {Z} {M})" : $"POINT z ({X} {Y} {Z})";
            return M.HasValue ? $"POINT m ({X} {Y} {M})" : $"POINT ({X} {Y})";
        }

        public override Geometry Union(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public override bool Within(Geometry geo)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return double.Equals(p1.X, p2.X) && double.Equals(p1.Y, p2.Y) && double.Equals(p1.Z, p2.Z) && double.Equals(p1.M, p2.M);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var p = obj as Point;
            if (!Is3D)
            {
                var temp = p.X == X && p.Y == Y;
                return p.X == X && p.Y == Y;
            }
            return p.X == X && p.Y == Y && p.Z == Z && p.M == M;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
