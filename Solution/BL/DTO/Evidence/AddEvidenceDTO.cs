using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace BL.DTO.Evidence
{
    public class AddEvidenceDTO
    {
        [Required(ErrorMessage = "Type is required")]
        public EvidenceType Type { get; set; }

        [Required(ErrorMessage = "File is required")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Capture Date is required")]
        public DateTime CaptureDate { get; set; }
        public string? Description { get; set; }
    }
}