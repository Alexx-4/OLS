using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OpenLatino.Core.Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public virtual ICollection<Client> RegisterApps { get; set; }
        public virtual ICollection<WorkSpace> CreatedWorkSpaces { get; set; }
        [NotMapped]
        public string UserId { get { return this.Id; } }
    }
}
