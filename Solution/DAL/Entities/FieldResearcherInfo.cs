using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities
{
    public class FieldResearcherInfo
    {
        [Required]
        public bool AvailabilityStatus { get; set; }

        [Key]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual AppUser User { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
