using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives.Geometry
{
    public abstract class MultiCurve : GeometryCollection
    {
        public IList<Curve> Curves { get { return list.Cast<Curve>().ToList(); } }
        public MultiCurve(IEnumerable<Curve> curves) : base(curves)
        {
            Dimension = 1;
            IsSimple = Curves.All(curve => curve.IsSimple);
            //TODO: falta comprobar que la única intersección entre cualquier dos elementos ocurre en un punto en los límites de ambos elementos
        }

        private bool? _isClosed = null;
        public virtual bool IsClosed
        {
            get
            {
                if (!_isClosed.HasValue)
                    _isClosed = Curves.All(curve => curve.IsClosed);
                return _isClosed.Value;
            }
        }

        public virtual double Length { get { return Curves.Sum(curve => curve.Length); } }
    }
}
