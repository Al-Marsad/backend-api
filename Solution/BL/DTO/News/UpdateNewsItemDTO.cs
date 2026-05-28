using System.ComponentModel.DataAnnotations;

namespace BL.DTO.News
{
    public class UpdateNewsItemDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }
        
        [Required(ErrorMessage = "Summary is required")]
        public string? Summary { get; set; }
        
        [Required(ErrorMessage = "Body is required")]
        public string? Body { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        public string? ImageUrl { get; set; }
    }
}