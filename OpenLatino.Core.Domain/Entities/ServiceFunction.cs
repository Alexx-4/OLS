using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLatino.Core.Domain.Entities
{
    public class ServiceFunction
    {
        public ServiceFunction()
        {
            this.WorkspaceFunctions = new HashSet<WorkspaceFunction>();
        }

        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Name { get; set; }

        public virtual Service Service { get; set; }
        public virtual ICollection<WorkspaceFunction> WorkspaceFunctions { get; set; }
    }
}