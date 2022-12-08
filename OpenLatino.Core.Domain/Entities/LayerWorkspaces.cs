using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Entities
{
    public class LayerWorkspaces
    {
        public int WorkSpaceId { get; set; }
        public int LayerId { get; set; }
        public int? VectorStyleId { get; set; }

        public virtual WorkSpace Workspace { get; set; }
        public virtual Layer Layer { get; set; }
        public virtual VectorStyle Style { get; set; }
    }
}
