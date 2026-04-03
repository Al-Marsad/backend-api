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
        public string Status { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
