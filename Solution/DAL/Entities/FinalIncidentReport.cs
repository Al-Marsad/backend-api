using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entities
{
    public class FinalIncidentReport
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public FinalIncidentReportStatus? Status { get; set; }
        
        public string? Url { get; set; }
        public bool DocumentationConsent { get; set; }
        public bool PublicationConsent { get; set; }

        [ForeignKey(nameof(Incident))]
        public int IncidentId { get; set; }
        
        [ForeignKey(nameof(FieldResearcher))]
        public string FieldResearcherId { get; set; }

        public virtual Incident Incident { get; set; }  
        public virtual FieldResearcherInfo FieldResearcher { get; set; }

        public virtual LegalReview? LegalReview { get; set; }    
    }
}
