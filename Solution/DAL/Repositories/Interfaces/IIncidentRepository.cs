using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces.Basic;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IIncidentRepository : ICreateRepository<Incident>, ISaveRepository, IGetByIdRepository<Incident>
    {
        public Task<(List<Incident>, int)> GetIncidentsByPageAndUserIdAsync(int skip, int take, string userId, string role,
            int? cityId, string? searchVictimNationalId, bool OrderByDateOfOccurence,bool? DocumentationConsent, 
            bool? PublicationConsent, bool? PreventModification, int? Sensitivity);
        public Task<(List<Incident>, int)> GetAllIncidentsByPageAsync(int skip, int take, int? cityId,
            string? searchVictimNationalId, bool OrderByDateOfOccurence, bool? DocumentationConsent, 
            bool? PublicationConsent, bool? PreventModification, int? Sensitivity);
        public Task<Incident?> GetFullByIdAsync(int id);
        public Task AddRangeOfEvidencesAsync(List<Evidence> evidences);

        public Task<List<Evidence>> GetEvidencesByIncidentIdAsync(int incidentId, EvidenceType? type = null);
        public Task<List<PersonalVictimTestimonie>> GetTestimoniesAndTheirVictimsByIncidentIdAsync(int incidentId);
        public Task<Incident?> GetWithTestimoniesOnlyById(int id);
        public Task<IncidentStatsModel> GetStatsAsync();
        public Task<MyIncidentStatsModel> GetMyStatsAsync(string userId);
        public Task<IncidentBrowseStatsModel> GetBrowseStatsAsync();
        public Task<PublicStatsModel> GetPublicStatsAsync();
        public Task<AnalyticsModel> GetAnalyticsAsync();
    }
}
