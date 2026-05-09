using System.ComponentModel.DataAnnotations;
using BL.Attributes.ValidationAttributes;
using DAL.Enums;

namespace BL.DTO.Victimm
{
    public class AddVictimDTO
    {
        [Required(ErrorMessage = "First name is required")  ]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Second name is required")]
        public string? SecondName { get; set; }

        [Required(ErrorMessage = "Thrid name is required")]
        public string? ThirdName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "National Id is required")]
        public string? NationalId { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Birthdate is required")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Marital Status is required")]
        public MaritalStatus MaritalStatus { get; set; }

        [Required(ErrorMessage = "Fmaily Size is required")]
        public int? FmailySize { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
