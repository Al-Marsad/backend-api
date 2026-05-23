

using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entities
{
    public class Activity
    {
        public int Id { get; set; } 
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public ActivityType Type { get; set; }

        
        [ForeignKey(nameof(MadeBy))]
        public string MadeById { get; set; }
        public AppUser MadeBy { get; set; }
    }
}
