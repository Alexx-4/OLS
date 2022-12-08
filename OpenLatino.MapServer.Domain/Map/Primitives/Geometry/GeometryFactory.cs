using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class GeometryFactory : IGeometryFactory
    {
        private WKTReader wktReader = new WKTReader();

        public Geometry GetGeometryFromWKT(string text)
        {
            return wktReader.GetGeometry(text);
        }
    }
}
