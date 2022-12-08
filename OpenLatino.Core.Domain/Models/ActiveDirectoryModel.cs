using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class ActiveDirectoryModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ClientId { get; set; }
    }
}
