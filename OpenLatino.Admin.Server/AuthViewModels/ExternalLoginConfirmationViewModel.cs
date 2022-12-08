using System.ComponentModel.DataAnnotations;

namespace OpenLatino.Admin.Server.AuthViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
