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
        public EvidenceType Type { get; set; }
        public string Url { get; set; }
        public DateTime CaptureDate { get; set; }
        public string? Description { get; set; }

        
        [ForeignKey(nameof(Incident))]
        public int? IncidentId { get; set; }
        public virtual Incident Incident { get; set; }
    }
}
