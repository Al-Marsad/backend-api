
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace BL.DTO.Victim
{
    public class ReturnVictimTestimonieDTO
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public string? PersonalNarrative { get; set; }
        public InjuryStatus InjuryStatus { get; set; }
        public string InjuryDescription { get; set; }
        public int IncidentId { get; set; }
        public ReturnVictimDTO Victim { get; set; }
    }
}
