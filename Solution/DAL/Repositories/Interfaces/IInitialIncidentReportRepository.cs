
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IInitialIncidentReportRepository : ICreateRepository<InitialIncidentReport>, 
        ISaveRepository, 
        IGetByIdRepository<InitialIncidentReport>
    {
        public Task<List<InitialIncidentReport>> GetPageAsync(int Skip, int Take, string? userId, InitialIncidentReportStatus? status, int? CityId);
    }
}
