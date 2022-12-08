using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Entities
{
    public class WorkspaceFunction
    {        
        public int WorkSpaceId { get; set; }
        public int FunctionId { get; set; }

        public virtual WorkSpace WorkSpace { get; set; }
        public virtual ServiceFunction Function { get; set; }
    }
}
