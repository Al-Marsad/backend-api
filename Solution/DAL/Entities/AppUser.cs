
using System.ComponentModel.DataAnnotations;
using DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string SecondName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string ThirdName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;    
        
        [Required]
        public DateTime Birthdate { get; set; }

        [MaxLength(500)]
        public string? RefreshToken { get; set; }
        
        public DateTime? RefreshTokenExpirationTime { get; set; }

        public virtual FieldResearcherInfo? ResearcherInfo { get; set; }

        public virtual List<InitialIncidentReport>? InitialIncidentReports { get; set; } 

        public virtual List<LegalReview>? LegalReviews { get; set; }
        public virtual List<NewsItem>? News { get; set; }
    }
}
