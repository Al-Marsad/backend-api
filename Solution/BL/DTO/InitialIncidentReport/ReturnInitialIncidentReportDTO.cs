
namespace BL.DTO.InitialIncidentReport
{
    public class ReturnInitialIncidentReportDTO
    {
        public int Id { get; set; } 
        public string Status { get; set; }
        public int CityId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
