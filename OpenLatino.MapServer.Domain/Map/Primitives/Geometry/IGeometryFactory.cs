using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public interface IGeometryFactory
    {
        Geometry GetGeometryFromWKT(string text);
    }
}
