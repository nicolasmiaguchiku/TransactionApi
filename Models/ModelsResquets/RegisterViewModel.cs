using System.ComponentModel.DataAnnotations;

namespace TransactionsApi.Models.ModelsResquets
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "E-mail is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
