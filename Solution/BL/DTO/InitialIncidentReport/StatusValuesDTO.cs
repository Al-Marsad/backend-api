using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;

namespace BL.DTO.InitialIncidentReport
{
    public class StatusValuesDTO
    {
        public InitialIncidentReportStatus Status { get; set; }
        public string StatuName{ get; set; }
    }
}
