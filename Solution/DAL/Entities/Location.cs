using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace DAL.Entities
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string AreaName { get; set; }
        
        public AreaClass? Class { get; set; }

        [Required]
        [MaxLength(50)]
        public string Coordinates { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        public virtual List<Incident> Incidents { get; set; }
    }
}
