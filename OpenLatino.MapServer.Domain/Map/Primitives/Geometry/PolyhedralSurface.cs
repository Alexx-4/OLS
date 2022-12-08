using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class PolyhedralSurface : Surface
    {
        public IList<Polygon> Patches { get; private set; }

        public int NumPatches { get { return Patches.Count; } }

        public Polygon this[int N] { get { return Patches[N]; } }

        public PolyhedralSurface(IEnumerable<Polygon> patches)
        {
            Patches = new List<Polygon>(patches);
        }

        public MultiPolygon BoundingPolygon(Polygon p)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
