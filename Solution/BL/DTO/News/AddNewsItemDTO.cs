using System.ComponentModel.DataAnnotations;

namespace BL.DTO.News
{
    public class AddNewsItemDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Summary is required")]
        public string? Summary { get; set; }
        
        [Required(ErrorMessage = "Body is required")]
        public string? Body { get; set; }
        
        [Required(ErrorMessage = "Image URL is required")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Incident ID is required")]
        public int? IncidentId { get; set; }
    }
}
