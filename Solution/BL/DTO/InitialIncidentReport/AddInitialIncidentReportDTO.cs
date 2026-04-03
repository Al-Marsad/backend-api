using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO.InitialIncidentReport
{
    public class AddInitialIncidentReportDTO
    {
        [Required(ErrorMessage = "Initial Description is required")]
        public string? InitialDescription { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double? LocationLat { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double? LocationLng { get; set; }

        public string? LocationLabel { get; set; }

        public string? WitnessName { get; set; }
        
        [Phone]
        public string? WitnessPhone { get; set; }

        [Required]
        public string? CitizenReporterId { get; set; }
    }
}
