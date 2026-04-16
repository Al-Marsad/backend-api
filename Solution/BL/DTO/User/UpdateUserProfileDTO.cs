using System.ComponentModel.DataAnnotations;

namespace BL.DTO.User
{
    public class UpdateUserProfileDTO
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
    }
}
