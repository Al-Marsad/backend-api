

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
        public string DetailedDescriptionStr { get; set; }
        public int? WitnessCount { get; set; }
        public string? WitnessDetailsStr { get; set; }

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

    }
}
