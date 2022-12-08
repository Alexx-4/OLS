using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class Tin : PolyhedralSurface
    {
        public Tin(IEnumerable<Triangle> triangles) : base(triangles)
        {

        }
    }
}
