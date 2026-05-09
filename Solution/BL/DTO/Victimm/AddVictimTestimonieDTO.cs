using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DAL.Entities;
using DAL.Enums;

namespace BL.DTO.Victimm
{
    public class AddVictimTestimonieDTO
    {
        [Required(ErrorMessage = "Issue date is required")]
        public DateTime IssueDate { get; set; }
        public string? PersonalNarrative { get; set; }
        public InjuryStatus InjuryStatus { get; set; }
        public string InjuryDescription { get; set; }

        // The priority is to NationalId, if both are not provided, we will return validation error
        public string? NationalId { get; set; } // If NationalId provided, there must not be a value in VictimDTO property
        public AddVictimDTO? Victim { get; set; } // if VictimDTO provided, there must not be a value in NationalId property

        [JsonIgnore]
        public int? VictimId { get; set; }

    }
}
