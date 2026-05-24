using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO.Victim
{
    public class ReturnUpdatedTestimonyDTO
    {
        public int Id { get; set; }
        public string? PersonalNarrative { get; set; }
        public int IncidentId { get; set; }
        public int VictimId { get; set; }


    }
}
