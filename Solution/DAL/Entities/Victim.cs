using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enums;

namespace DAL.Entities
{
    public class Victim
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string SecondName { get; set; }

        [MaxLength(50)]
        public string ThirdName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string NationalId { get; set; }

        public Gender Gender { get; set; }

        public DateTime Birthdate { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }

        public int? FmailySize { get; set; }

        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        public virtual List<PersonalVictimTestimonie> PersonalVictimTestimonies { get; set; }
    }
}
    