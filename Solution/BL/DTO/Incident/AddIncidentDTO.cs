using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BL.DTO.Victim;
using DAL.Enums;

namespace BL.DTO.Incident
{
    public class AddIncidentDTO
    {
        [Required(ErrorMessage = "DateOfOccurrence is required")]
        public DateTime DateOfOccurrence { get; set; }
        
        [Required(ErrorMessage = "DetailedDescription is required")]
        public string DetailedDescription { get; set; }

        [Required(ErrorMessage = "AreaName is required")]
        public string AreaName { get; set; }

        [Required(ErrorMessage = "AreaClass is required")]
        public AreaClass AreaClass { get; set; }
        
        [Required(ErrorMessage = "AreaType is required")]
        public AreaType AreaType { get; set; }
        public string? LocationDescription { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double LocationLat { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double LocationLng { get; set; }
        public string? PerpetratorDescription { get; set; }

        [Required(ErrorMessage = "SensitivityScore is required")]
        [Range(1,10)]
        public byte SensitivityScore { get; set; }

        [Required(ErrorMessage = "SensitivityScore is required")]
        public string? QuestionnaireJSON { get; set; }
        public int CityId { get; set; }
        
        public int? InitialIncidentReportId { get; set; }

        [JsonIgnore]
        public string? FieldResearcherId { get; set; }

        public List<AddVictimTestimonieDTO>? PersonalVictimTestimonies { get; set; }

    }
}
