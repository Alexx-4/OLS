using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenLatino.Core.Domain.Entities
{
    public class WorkSpace
    {
        public WorkSpace()
        {
            CLientWorkSpaces = new HashSet<ClientWorkSpaces>();
            LayerWorkspaces = new HashSet<LayerWorkspaces>();
            ServiceFunctions = new HashSet<WorkspaceFunction>();
            Default = false; 
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Default { get; set; }
        public string ForAppType { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        public virtual ICollection<ClientWorkSpaces> CLientWorkSpaces { get; set; }
        public virtual ICollection<LayerWorkspaces> LayerWorkspaces { get; set; }
        public virtual ICollection<WorkspaceFunction> ServiceFunctions { get; set; }
    }
}