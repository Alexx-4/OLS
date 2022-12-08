using System;
using System.Collections.Generic;

namespace OpenLatino.Core.Domain.Entities
{
    public partial class ClientWorkSpaces
    {
        public string ClientId { get; set; }
        public int WorkSpaceId { get; set; }

        public virtual Client Client { get; set; }
        public virtual WorkSpace WorkSpace { get; set; }
    }
}
