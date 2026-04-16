using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace BL.DTO.InitialIncidentReport
{
    public class GetByPageInitialIncidentReportDTO
    {
        public InitialIncidentReportStatus? Status { get; set; }
        public int? CityId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than or equal to 1.")]
        public int Page { get; set; } = 1;

        [Range(0, 50, ErrorMessage = "PageSize must be between 0 and 50.")]
        public int PageSize { get; set; } = 20;
    }
}
