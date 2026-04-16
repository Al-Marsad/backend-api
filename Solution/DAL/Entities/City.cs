
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DAL.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public virtual List<AppUser> Users { get; set; } = new();

        public virtual List<Location> Locations { get; set; } = new();
        public virtual List<InitialIncidentReport> InitialIncidentReports { get; set; } = new();
    }
}
