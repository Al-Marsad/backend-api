using DAL.Entities;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IIncidentRepository : ICreateRepository<Incident>, ISaveRepository, IGetByIdRepository<Incident>
    {
        public Task<(List<Incident>, int)> GetPageAsync(int skip, int take, string userId, string? searchVictimNationalId);
        public Task<Incident?> GetFullByIdAsync(int id);
        public Task AddRangeOfEvidences(List<Evidence> evidences);



    }
}
