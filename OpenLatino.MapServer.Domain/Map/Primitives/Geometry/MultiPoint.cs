using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public class MultiPoint : GeometryCollection
    {
        private MultiPoint(IEnumerable<Point> points) : base(points)
        {
            bool flag = false;
            for (int i = 0; !flag && i < NumGeometries - 1; i++)
                for (int j = i + 1; !flag && j < NumGeometries; j++)
                    flag = this[i].Equals(this[j]);
            IsSimple = flag;
        }

        public override Geometry Boundary()
        {
            //retornar el conjunto vacío
            throw new NotImplementedException();
        }
    }
}
