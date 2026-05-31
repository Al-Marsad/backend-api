
using System.ComponentModel.DataAnnotations;
using BL.Models;

namespace BL.DTO.General
{
    public class SeedVectorDatabaseDTO
    {
        [Required]
        public List<RomeStatuteArticle> Articles { get; set; }
    }
}
