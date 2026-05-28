using DAL.Enums;

namespace BL.DTO.News
{
    public class ReturnNewsIncidentLocationDTO
    {
        public int IncidentId { get; set; }
        public DateTime DateOfOccurrence { get; set; }
        public string AreaName { get; set; }
        public AreaClass AreaClass { get; set; }
        public AreaType AreaType { get; set; }
        public string? LocationDescription { get; set; }
        public double LocationLat { get; set; }
        public double LocationLng { get; set; }
        public int CityId { get; set; }
    }
}
