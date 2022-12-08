using Microsoft.AspNetCore.Identity;
using OpenLatino.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }
        [Display(Name = "Phone Confirmed")]
        public bool PhoneConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public List<WorkSpace> workSpaces { get; set; }
        public List<Client> clients { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<string> RolsIds { get; set; }
    }
}
