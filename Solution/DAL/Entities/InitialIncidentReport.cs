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
        public InitialIncidentReportStatus Status { get; set; } = InitialIncidentReportStatus.New;

        [Required]
        [MaxLength(2000)]
        public string InitialDescription { get; set; }

        [ForeignKey(nameof(CitizenReporter))]
        public string CitizenReporterId { get; set; }

        public virtual AppUser CitizenReporter { get; set; }

        public virtual List<Incident> Incidents { get; set; }
    }
}
