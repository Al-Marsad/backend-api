using BL.DTO.InitialIncidentReport;

namespace BL.Services.Interfaces
{
    public interface IInitialIncidentReportService
    {
        public Task<ReturnInitialIncidentReportDTO> AddAsync(AddInitialIncidentReportDTO reportDto);

        public Task<ReturnDetailedInitialIncidentReportDTO> GetByIdAsync(int id, string userId);
    }
}
