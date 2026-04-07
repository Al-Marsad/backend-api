using System.ComponentModel.DataAnnotations;

namespace BL.DTO.City
{
    public class AddCityDTO
    {
        [Required(ErrorMessage = "Arabic Name is rquired")]
        public string ArabicName { get; set; }

        [Required(ErrorMessage = "English Name is rquired")]
        public string EnglishName { get; set; }
    }
}
