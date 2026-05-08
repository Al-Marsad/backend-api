using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;

namespace BL.DTO.Incident
{
    public class AddIncidentDTO
    {
        public DateTime DateOfOccurrence { get; set; }
        public string DetailedDescription { get; set; }
        public string AreaName { get; set; }
        public AreaClass AreaClass { get; set; }
        public AreaType? AreaType { get; set; }
        public string? LocationDescription { get; set; }
        public double LocationLat { get; set; }
        public double LocationLng { get; set; }
        public string? PerpetratorDescription { get; set; }
        public byte SensitivityScore { get; set; }


        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
    }
}
