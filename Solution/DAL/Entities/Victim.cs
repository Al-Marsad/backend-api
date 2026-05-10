using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace DAL.Entities
{
    public class Victim
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public int FamilySize { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public virtual List<PersonalVictimTestimonie> PersonalVictimTestimonies { get; set; } = new();
    }
}
    