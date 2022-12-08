using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenLatino.Core.Domain.Entities
{
    public class Service
    {
        public Service()
        {
            ServiceFunctions = new HashSet<ServiceFunction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ServiceFunction> ServiceFunctions { get; set; }
    }
}