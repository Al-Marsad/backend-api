using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTO.Victim;

namespace BL.DTO.Incident
{
    public class ReturnUpdatedIncidentDTO
    {
        public int Id { get; set; }
        public string DetailedDescription { get; set; }
        public string? AIClassification { get; set; }
        public string? PerpetratorDescription { get; set; }
        public int? InitialIncidentReportId { get; set; }
        public string? FieldResearcherId { get; set; }
        public string? LegalTeamMemberId { get; set; }
        public bool PreventModification { get; set; }


        public List<ReturnUpdatedTestimonyDTO>? PersonalVictimTestimonies { get; set; }
    }
}
