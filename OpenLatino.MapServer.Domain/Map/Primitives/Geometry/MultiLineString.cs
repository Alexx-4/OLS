using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class MultiLineString : MultiCurve
    {
        public IList<LineString> Lines { get { return list as IList<LineString>; } }

        public MultiLineString(IEnumerable<LineString> lines) : base(lines)
        {

        }
    }
}
