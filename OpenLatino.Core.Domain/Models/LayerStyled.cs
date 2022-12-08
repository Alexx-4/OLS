using OpenLatino.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class LayerStyled
    {
        public int? LayerId { get; set; }
        public string LayerName { get; set; }
        public VectorStyle Style { get; set; }
    }
}
