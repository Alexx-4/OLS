
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Enums;

namespace OpenLatino.Core.Domain.Models
{
    public class ClientModel
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }


        public string ApplicationType { get; set; }

        [Display(Name = "Workspaces")]
        public List<WorkSpace> Workspaces { get; set; }

        public List<int> WorkspacesIds { get; set; }


        //[Required]
        //[MinLength(8)]
        //[MaxLength(255)]
        public string Password { get; set; }

        public string AllowedOrigen { get; set; }
    }
}
