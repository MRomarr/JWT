using System.ComponentModel.DataAnnotations;

namespace JWT.DTOs
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
    }
}
