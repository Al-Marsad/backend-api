
namespace BL.DTO.InitialIncidentReport
{
    public class ReturnDetailedInitialIncidentReportDTO
    {
        public int Id { get; set; }
        public string InitialDescription { get; set; }
        public double LocationLat { get; set; }
        public double LocationLng { get; set; }
        public int CityId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public string WitnessName { get; set; }
        public string WitnessPhone { get; set; }
    }
}
