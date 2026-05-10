using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entities
{
    public class Evidence
    {
        public int Id { get; set; }
        public EvidenceType Type { get; set; }
        public string CloudinaryUrl { get; set; }
        public string CloudinaryPublicId { get; set; }  
        public DateTime CaptureDate { get; set; }
        public string? Description { get; set; }

        
        [ForeignKey(nameof(Incident))]
        public int IncidentId { get; set; }
        public virtual Incident Incident { get; set; }
    }
}
