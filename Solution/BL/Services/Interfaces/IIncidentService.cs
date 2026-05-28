using BL.DTO.City;
using BL.DTO.Evidence;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.DTO.InitialIncidentReport;
using BL.DTO.Victim;
using BL.Helper;
using DAL.Entities;
using DAL.Enums;

namespace BL.Services.Interfaces
{
    public interface IIncidentService
    {
        public Task<ReturnFullIncidentDTO> AddAsync(AddIncidentDTO incidentDTO);
        public Task<PagedResultDTO<List<ReturnIncidentDTO>>> GetIncidentsByPageAndUserIdAsync(
            PaginationDTO pageDTO, CurrentUser user, int? cityId, string? searchVictimNationalId, bool OrderByDateOfOccurence,
            bool? DocumentationConsent, bool? PublicationConsent, bool? PreventModification, int? Sensitivity);
        public Task<PagedResultDTO<List<ReturnIncidentDTO>>> GetAllIncidentsByPageAsync(PaginationDTO pageDTO, int? cityId,
            string? searchVictimNationalId, bool OrderByDateOfOccurence, bool? DocumentationConsent, bool? PublicationConsent, 
            bool? PreventModification, int? Sensitivity);

        public Task<List<ReturnEvidenceDTO>> AddRangeOfRelatedEvidences(List<AddEvidenceDTO> evidenceDTOs, int incidentId);
        public Task<List<ReturnEvidenceDTO>> GetEvidencesByIncidentIdAsync(int incidentId, EvidenceType? type);

        public Task<ReturnIncidentDTO> GetByIdAsync(int Id);
        public Task<List<ReturnVictimTestimonieDTO>> GetTestimoniesAndTheirVictimsByIncidentIdAsync(int incidentId);

        public Task<ReturnAssignedIncidentDTO> AssignToLegalTeamMember(string userId, int IncidentId);
        public Task<ReturnAssignedIncidentDTO> UnassignToLegalTeamMember(string userId, int IncidentId);

        public Task<ReturnUpdatedIncidentDTO> UpdateIncident(UpdateIncidentDTO updateIncidentDTO, int IncidentId, string userId);
        public Task<ReturnGiveConsentDTO> GiveDocumentationConsentAsync(int IncidentId, string userId);
        public Task<ReturnGiveConsentDTO> GivePublicationConsentAsync(int IncidentId, string userId);

        public Task RequestModificationAsync(int IncidentId, string userId);

        public Task<ReturnGiveConsentDTO> AllowModificationAsync(int IncidentId, string userId);

    }
}
