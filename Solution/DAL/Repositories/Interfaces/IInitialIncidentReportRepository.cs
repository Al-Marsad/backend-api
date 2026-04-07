
using DAL.Entities;
using DAL.Enums;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IInitialIncidentReportRepository : ICreateRepository<InitialIncidentReport>, 
        ISaveRepository, 
        IGetByIdRepository<InitialIncidentReport>,
        IGetPageRepository<InitialIncidentReport>
    {
        public Task<List<InitialIncidentReport>> GetByStatusAsync(InitialIncidentReportStatus status);
    }
}
