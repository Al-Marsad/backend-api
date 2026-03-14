using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class LegalReview
    {
        public int Id { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }
        
        [Required]
        public string ReviewContent { get; set; }

        [ForeignKey(nameof(FinalIncidentReport))]
        public int FinalIncidentReportId { get; set; }

        [ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; }
        public virtual FinalIncidentReport FinalIncidentReport { get; set; }
        public virtual AppUser AppUser { get; set; }

    }
}
