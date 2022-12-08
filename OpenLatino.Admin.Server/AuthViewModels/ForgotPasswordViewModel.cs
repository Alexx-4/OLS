using System.ComponentModel.DataAnnotations;

namespace OpenLatino.Admin.Server.AuthViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
