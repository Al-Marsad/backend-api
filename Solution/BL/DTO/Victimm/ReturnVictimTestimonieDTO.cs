using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Enums;

namespace BL.DTO.Victimm
{
    public class ReturnVictimTestimonieDTO
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public string? PersonalNarrative { get; set; }
        public InjuryStatus InjuryStatus { get; set; }
        public string InjuryDescription { get; set; }
        public int IncidentId { get; set; }
        public int VictimId { get; set; }
        public virtual ReturnVictimDTO Victim { get; set; }
    }
}
