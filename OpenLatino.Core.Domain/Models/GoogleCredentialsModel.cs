using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class GoogleCredentialsModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string GoogleId { get; set; }
    }
}
