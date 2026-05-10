using BL.DTO.City;
using BL.DTO.Evidence;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.DTO.InitialIncidentReport;
using BL.Helper;
using DAL.Entities;

namespace BL.Services.Interfaces
{
    public interface IIncidentService
    {
        public Task<ReturnFullIncidentDTO> AddAsync(AddIncidentDTO incidentDTO);
        public Task<PagedResultDTO<List<ReturnIncidentDTO>>> GetByPageAsync(
            PaginationDTO pageDTO, string userId, string? searchVictimNationalId);

        public Task<List<ReturnEvidenceDTO>> AddRangeOfRelatedEvidences(List<AddEvidenceDTO> evidenceDTOs, int incidentId);
        public Task<List<ReturnEvidenceDTO>> GetEvidencesByIncidentIdAsync(int incidentId);

    }
}
