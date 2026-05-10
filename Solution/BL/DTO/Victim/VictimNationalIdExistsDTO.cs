using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTO.Victim
{
    public  class VictimNationalIdExistsDTO
    {
        [Required(ErrorMessage = "National Id is required")]
        public string NationalId { get; set; } 
    }
}
