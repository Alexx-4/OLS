using OpenLatino.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class WLayerStyleEditor
    {
        public string clientId { get; set; }
        public int Wid { get; set; }
        public int Lid { get; set; }
        public int Sid { get; set; } //For the resulting style
        public IEnumerable<VectorStyle> Styles { get; set; }
    }
}
