using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Entities
{
    public class LayerStyle
    {
        public int LayerId { get; set; }
        public int VectorStyleId { get; set; }
        public virtual Layer Layer { get; set; }
        public virtual VectorStyle VectorStyle { get; set; }
    }
}
