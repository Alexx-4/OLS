using OpenLatino.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class ClienFullInfo
    {
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string ApplicationType { get; set; }
        public bool Active { get; set; }
        public string AccessKey { get; set; }
        public string UpdateKey { get; set; }
        public List<WorkSpace> workSpaces { get; set; }
        public List<int> WorkspacesIds { get; set; }
        public ClienFullInfo()
        {
            this.workSpaces = new List<WorkSpace>();
            this.WorkspacesIds = new List<int>();
        }
    }
}
