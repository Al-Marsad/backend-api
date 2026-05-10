using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTO.Victim;
using DAL.Enums;

namespace BL.DTO.Incident
{
    public class ReturnFullIncidentDTO
    {
        public int Id { get; set; }
        public DateTime DateOfOccurrence { get; set; }
        public DateTime CreationDate { get; set; }
        public string DetailedDescription { get; set; }
        public string AreaName { get; set; }
        public AreaClass AreaClass { get; set; }
        public AreaType AreaType { get; set; }
        public string? LocationDescription { get; set; }
        public double LocationLat { get; set; }
        public double LocationLng { get; set; }
        public string? PerpetratorDescription { get; set; }
        public byte SensitivityScore { get; set; }
        public string? QuestionnaireJSON { get; set; }
        public int CityId { get; set; }
        public int? InitialIncidentReportId { get; set; }
        public string? FieldResearcherId { get; set; }

        public List<ReturnAbbreviatedVictimTestimonieDTO> PersonalVictimTestimonies { get; set; }

    }
}
