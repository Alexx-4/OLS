using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBush;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public enum GeometryType
    {
        Point,
        GeometryCollection,
        LineString,
        MultiLineString,
        LinearRing,
        Polygon,
        Circle,
    }

    public abstract class Geometry : Element
    {
        public Geometry()
        {
            IsVisible = true;
            Type = ElementType.Geometry;
        }

        //TODO: Completar con sistemas de referencias y de medidas
        public int Dimension { get; set; }

        public int CoordinateDimension { get; set; }

        public int SpatialDimension { get; set; }

        public GeometryType GeometryType { get; set; }

        public int SRID { get; set; }

        public bool IsEmpty { get; set; }

        public bool IsSimple { get; set; }

        public bool Is3D { get; set; }

        public bool IsMeasured { get; set; }

        public abstract Envelope Envelope();

        public abstract byte[] ToBinary();

        public abstract Geometry Boundary();

        public abstract string ToWKTString();

        public Dictionary<string, object> Attributes { get; set; }


        #region Query

        public abstract bool Equals(Geometry geo);

        public abstract bool Disjoint(Geometry geo);

        public abstract bool Intersects(Geometry geo);

        public abstract bool Touches(Geometry geo);

        public abstract bool Crosses(Geometry geo);

        public abstract bool Within(Geometry geo);

        public abstract bool Contains(Geometry geo);

        public abstract bool Overlaps(Geometry geo);

        public abstract bool Relate(Geometry geo, string matrix);

        public abstract Geometry LocatoAlong(double mValue);

        public abstract Geometry LocateBetween(double mStart, double mEnd);

        #endregion

        #region Analysis

        public abstract double Distance(Geometry geo);

        public abstract Geometry Buffer(double distance);

        public abstract Geometry ConvexHull();

        public abstract Geometry Intersection(Geometry geo);

        public abstract Geometry Union(Geometry geo);

        public abstract Geometry Difference(Geometry geo);

        public abstract Geometry SymDifference(Geometry geo);

        #endregion
    }
}
