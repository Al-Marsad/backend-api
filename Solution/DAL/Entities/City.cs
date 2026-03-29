
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DAL.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public virtual List<FieldResearcherInfo> FieldResearchers { get; set; } = new();

        public virtual List<Location> Locations { get; set; } = new();
    }
}
