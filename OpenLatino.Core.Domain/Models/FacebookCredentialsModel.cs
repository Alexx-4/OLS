using System.ComponentModel.DataAnnotations;

namespace OpenLatino.Core.Domain.Models
{
    public class FacebookCredentialsModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string FacebookId { get; set; }

        [Required]
        public string FacebookToken { get; set; }
    }
}
