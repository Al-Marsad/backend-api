using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO.Incident
{
    public class ReturnGiveDocumentationConsentDTO
    {
        public int IncidentId { get; set; }
        public bool DocumentationConsent { get; set; }
        public bool PreventModification { get; set; }
    }
}
