using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;

namespace BL.DTO.Evidence
{
    public class ReturnEvidenceDTO
    {
        public int Id { get; set; }
        public EvidenceType Type { get; set; }
        public string CloudinaryUrl { get; set; }
        public string CloudinaryPublicId { get; set; }
        public DateTime CaptureDate { get; set; }
        public string Description { get; set; }
        public int IncidentId { get; set; }
    }
}
