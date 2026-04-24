using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace BL.DTO.User
{
    public class UpdateFullUserAccountDTO
    {
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Second name is required")]
        public string? SecondName { get; set; }

        [Required(ErrorMessage = "Thrid name is required")]
        public string? ThirdName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "City Id is required")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [RegularExpression(
        @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one digit, " +
            "one lowercase letter, one uppercase letter, and one non‑alphanumeric symbol.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "ResetPassword field is required")]
        public bool? ResetPassword { get; set; }
    }
}
