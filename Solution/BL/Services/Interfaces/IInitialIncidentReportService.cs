using BL.DTO.InitialIncidentReport;
using BL.Helper;
using DAL.Enums;

namespace BL.Services.Interfaces
{
    public interface IInitialIncidentReportService
    {
        public Task<ReturnInitialIncidentReportDTO> AddAsync(AddInitialIncidentReportDTO reportDto);
        public Task<ReturnDetailedInitialIncidentReportDTO> GetByIdAsync(int id, string userId);
        public Task<List<ReturnDetailedInitialIncidentReportDTO>> GetByPageAsync(GetByPageInitialIncidentReportDTO reportDTO
            ,CurrentUser user);
        public List<StatusValuesDTO> GetStatusValues();

        public Task<ReturnInitialIncidentReportDTO> AssignToFieldResearcher(AssignToFieldResearcherDTO data);
    }
}
