using DAL.Entities;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IIncidentRepository : ICreateRepository<Incident>, ISaveRepository, IGetByIdRepository<Incident>
    {
        public Task<(List<Incident>, int)> GetFieldResearcherIncidentsByPageAsync(int skip, int take, string userId, 
            string? searchVictimNationalId, bool OrderByDateOfOccurence,
            bool DocumentationConsent, bool PublicationConsent);
        public Task<(List<Incident>, int)> GetAllIncidentsByPageAsync(int skip, int take, int? cityId
            ,bool OrderByDateOfOccurence, bool Approved, int? Sensitivity);
        public Task<Incident?> GetFullByIdAsync(int id);
        public Task AddRangeOfEvidencesAsync(List<Evidence> evidences);

        public Task<List<Evidence>> GetEvidencesByIncidentIdAsync(int incidentId);
        public Task<List<PersonalVictimTestimonie>> GetTestimoniesAndTheirVictimsByIncidentIdAsync(int incidentId);
        public Task<Incident?> GetWithTestimoniesOnlyById(int id);
    }
}
