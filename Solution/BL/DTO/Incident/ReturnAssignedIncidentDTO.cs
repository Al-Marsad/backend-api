using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO.Incident
{
    public class ReturnAssignedIncidentDTO
    {
        public int Id { get; set; }
        public int? InitialIncidentReportId { get; set; }
        public string? FieldResearcherId { get; set; }
        public string? LegalTeamMemberId { get; set; }
        public DateTime CreationDate { get; set; }


    }
}
