using OpenLatino.Core.Domain.Entities;
using System.Collections.Generic;

namespace OpenLatino.Core.Domain.Models
{
    public class WorkspaceFullInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Client> Clients { get; set; }
        public ICollection<LayerStyled> LayerStyled { get; set; }
        public ICollection<ServiceFunction> ServiceFunctions { get; set; }
    }
}
