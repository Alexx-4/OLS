using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class ClientCredentialModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string AccessKey { get; set; }
        [Required]
        public string UpdateKey { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string AllowedOrigen { get; set; }
        public long ExpirationDate { get; set; }
    }
}
