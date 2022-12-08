using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class RegisterExternalBindingModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string ExternalAccessToken { get; set; }

        [Required]
        public string ClientId { get; set; }
    }
}
