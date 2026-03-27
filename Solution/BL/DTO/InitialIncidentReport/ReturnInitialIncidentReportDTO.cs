using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO.InitialIncidentReport
{
    public class ReturnInitialIncidentReportDTO
    {
        public int Id { get; set; } 
        public string InitialDescription { get; set; }
        public string CitizenReporterId { get; set; }

        public string StatusName { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
