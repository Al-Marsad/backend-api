using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Enums;

namespace BL.DTO.Victim
{
    public class ReturnAbbreviatedVictimTestimonieDTO
    {
        public int Id { get; set; }
        public int IncidentId { get; set; }
        public int VictimId { get; set; }
        public virtual ReturnAbbreviatedVictimDTO Victim { get; set; }
    }
}
