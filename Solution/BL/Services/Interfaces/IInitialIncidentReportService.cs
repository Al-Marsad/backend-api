using BL.DTO.InitialIncidentReport;
using DAL.Enums;

namespace BL.Services.Interfaces
{
    public interface IInitialIncidentReportService
    {
        public Task<ReturnInitialIncidentReportDTO> AddAsync(AddInitialIncidentReportDTO reportDto);

        public Task<ReturnDetailedInitialIncidentReportDTO> GetByIdAsync(int id, string userId);

        public Task<List<ReturnDetailedInitialIncidentReportDTO>> GetByPageAsync(int page, int pageSize, string userId);

        public Task<List<ReturnDetailedInitialIncidentReportDTO>> GetByPageAsync(int page, int pageSize, string userId,
            InitialIncidentReportStatus status);

    }
}
