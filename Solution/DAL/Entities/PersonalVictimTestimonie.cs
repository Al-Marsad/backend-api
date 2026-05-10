using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entities
{
    public class PersonalVictimTestimonie
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public string? PersonalNarrative { get; set; }
        public InjuryStatus InjuryStatus { get; set; }
        public string? InjuryDescription { get; set; }



        [ForeignKey(nameof(Incident))]
        public int IncidentId { get; set; }
        public virtual Incident Incident { get; set; }
        
        
        [ForeignKey(nameof(Victim))]
        public int VictimId { get; set; }
        public virtual Victim Victim { get; set; }

    }
}