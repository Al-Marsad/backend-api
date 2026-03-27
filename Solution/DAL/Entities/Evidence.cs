using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;

namespace DAL.Entities
{
    public class Evidence
    {
        public int Id { get; set; }

        [Required]
        public EvidenceType Type { get; set; }
        
        
        [MaxLength(2048)]  
        [Required]
        public string Url { get; set; }

        [Required]
        public DateTime CaptureDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [ForeignKey(nameof(Incident))]
        public int? IncidentId { get; set; }
        
        [ForeignKey(nameof(Residence))]
        public int? ResidenceId { get; set; }
        public virtual Incident Incident { get; set; }
        public virtual Residence Residence { get; set; }
    }
}
