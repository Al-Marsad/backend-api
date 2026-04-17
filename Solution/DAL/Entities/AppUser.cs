
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string SecondName { get; set; }
        
        [Required]
        public string ThirdName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;    
        
        [Required]
        public DateTime Birthdate { get; set; }

        public bool? AvailabilityStatus { get; set; }


        public string? RefreshToken { get; set; }
        
        public DateTime? RefreshTokenExpirationTime { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public virtual List<InitialIncidentReport> InitialIncidentReports { get; set; } = new();
        public virtual List<InitialIncidentReport> AssignedInitialReports { get; set; } = new();
        public virtual List<LegalReview> LegalReviews { get; set; } = new();

        public virtual List<NewsItem> News { get; set; } = new();

        public virtual List<Incident> Incidents { get; set; } = new();

        public virtual List<FinalIncidentReport> FinalIncidentReports { get; set; } = new();
    }
}
