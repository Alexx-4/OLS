using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class TematicInfo
    {
        public int tematicId { get; set; }
        public string tematicName { get; set; }
        public string tematicType { get; set; }

        public int layerId { get; set; }
        public string layerName { get; set; }
        public string tableName { get; set; }

        public int tematicTypeId { get; set; }
        public string tematicTypeName { get; set; }

        public int styleId { get; set; }
        public string styleName { get; set; }

    }
}
