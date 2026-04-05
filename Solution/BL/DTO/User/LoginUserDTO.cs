using System.ComponentModel.DataAnnotations;

namespace BL.DTO.User
{
    public class LoginUserDTO
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and contain Uppercase, Lowercase, Number, and Special character.")]
        public string Password { get; set; }
    }
}
