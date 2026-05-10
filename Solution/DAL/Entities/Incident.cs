

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entities
{
    public class Incident
    {
        public int Id { get; set; }
        public DateTime DateOfOccurrence { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public string DetailedDescription { get; set; }
        public int? WitnessCount { get; set; }
        public string? WitnessDetails { get; set; }
        public string AreaName { get; set; }    
        public AreaClass AreaClass { get; set; }
        public AreaType AreaType { get; set; }
        public string? LocationDescription { get; set; }
        public double LocationLat { get; set; }
        public double LocationLng { get; set; }
        public string? PerpetratorDescription { get; set; }
        public byte SensitivityScore { get; set; }
        public string? QuestionnaireJSON { get; set; }


        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public virtual City City { get; set; }


        [ForeignKey(nameof(InitialIncidentReport))]
        public int? InitialIncidentReportId { get; set; }
        public virtual InitialIncidentReport? InitialIncidentReport { get; set; }


        [ForeignKey(nameof(FieldResearcher))]
        public string FieldResearcherId { get; set; }
        public virtual AppUser FieldResearcher { get; set; }    


        public virtual FinalIncidentReport? FinalIncidentReport { get; set; }
        public virtual NewsItem? NewsItem { get; set; }
        public virtual List<Evidence> Evidences { get; set; } = new();
        public virtual List<PersonalVictimTestimonie> PersonalVictimTestimonies { get; set; } = new();
    }
}
