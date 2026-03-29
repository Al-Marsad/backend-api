

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Incident
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateOfOccurrence { get; set; }

        [Required]
        [Column(TypeName = "jsonb")]
        public Dictionary<string, object> DetailedDescription { get; set; } = new();
        
        public int WitnessCount { get; set; }

        [Column(TypeName = "jsonb")]
        public Dictionary<string, object> WitnessDetails { get; set; } = new();


        [ForeignKey(nameof(InitialIncidentReport))]
        public int? InitialIncidentReportId { get; set; }

        [ForeignKey(nameof(Location))]
        public int LocationId { get; set; }

        [ForeignKey(nameof(FieldResearcher))]
        public string FieldResearcherId { get; set; }
        public virtual InitialIncidentReport? InitialIncidentReport { get; set; }

        public virtual Location Location { get; set; }

        public virtual FieldResearcherInfo FieldResearcher { get; set; }    

        public virtual FinalIncidentReport? FinalIncidentReport { get; set; }

        public virtual NewsItem? NewsItem { get; set; }

        public virtual Residence? Residence { get; set; }

        public virtual List<Evidence> Evidences { get; set; } = new();
        public virtual List<PersonalVictimTestimonie> PersonalVictimTestimonies { get; set; } = new();
    }
}
