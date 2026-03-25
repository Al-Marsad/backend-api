using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class PersonalVictimTestimonie
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }

        public string PersonalNarrative { get; set; }

        [MaxLength(3000)]
        public string? HardestMoment { get; set; }
        
        [MaxLength(3000)]
        public string? InternationalDemand { get; set; }

        [ForeignKey(nameof(Incident))]
        public int IncidentId { get; set; }
        
        [ForeignKey(nameof(Victim))]
        public int VictimId { get; set; }
        public virtual Incident Incident { get; set; }
        public virtual Victim Victim { get; set; }

    }
}