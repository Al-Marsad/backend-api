using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entities
{
    public class InitialIncidentReport
    {
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public InitialIncidentReportStatus Status { get; set; } = InitialIncidentReportStatus.UNASSIGNED;

        [Required]
        public string InitialDescription { get; set; }

        public double LocationLat { get; set; }
        public double LocationLng { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public string? WitnessName { get; set; }

        [Phone]
        public string? WitnessPhone { get; set; }
        

        [ForeignKey(nameof(CitizenReporter))]
        public string CitizenReporterId { get; set; }
        
        [ForeignKey(nameof(FieldResearcher))]
        public string? FieldResearcherId { get; set; }

        public virtual AppUser CitizenReporter { get; set; }
        public virtual AppUser? FieldResearcher { get; set; }

        public virtual List<Incident> Incidents { get; set; } = new();
    }
}
