using System.ComponentModel.DataAnnotations;

namespace BL.DTO.User
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "CurrentPassword is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and contain Uppercase, Lowercase, Number, and Special character.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "NewPassword is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and contain Uppercase, Lowercase, Number, and Special character.")]
        public string NewPassword { get; set; }
    }
}
