using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTO.Victim;

namespace BL.DTO.Incident
{
    public class UpdateIncidentDTO
    {
        public string? DetailedDescription { get; set; }

        public string? AIClassification { get; set; }
        
        public string? PerpetratorDescription { get; set; }

        public List<UpdateTestimonyDTO>? Testimonies { get; set; }

    }
}
