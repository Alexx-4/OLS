using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Map.Primitives
{
    public enum ElementType
    {
        Geometry,
        Text,
        Raster,
        Icon,
        Data
    }

    public abstract class Element
    {
        public ulong? Id { get; set; }

        public ElementType Type { get; set; }

        public bool IsVisible { get; set; } = true;

        public int LayerId { get; set; }
    }
}
