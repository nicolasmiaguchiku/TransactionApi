using System.ComponentModel.DataAnnotations;

namespace TransactionsApi.Models.ModelsResquets
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-mail is required.")]
        [EmailAddress(ErrorMessage = "address e-mail is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]  
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } =string.Empty;
    }
}
