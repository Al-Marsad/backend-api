
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IInitialIncidentReportRepository : ICreateRepository<InitialIncidentReport>, 
        ISaveRepository, 
        IGetByIdRepository<InitialIncidentReport>,
        ICountRepository
    {
        public Task<(List<InitialIncidentReport>, int TotalItems)> GetPageAsync(int Skip, int Take, string? userId, 
            InitialIncidentReportStatus? status, int? CityId);
        public Task<(List<InitialIncidentReport>, int totalItems)> GetAssignedReportsAsync(string userId, int Skip,
            int Take, string? search);
    }
}
