using System.Collections.Generic;

namespace OpenLatino.Core.Domain.Models
{
    public class WorkspaceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApplicationUserId { get; set; }
        public IEnumerable<int> LayerIds { get; set; }
        public IEnumerable<int> FunctionIds { get; set; }
    }
}
