using System.ComponentModel.DataAnnotations;

namespace BL.DTO.User
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
