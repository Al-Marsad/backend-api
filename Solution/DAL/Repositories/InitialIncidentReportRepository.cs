using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class InitialIncidentReportRepository : IInitialIncidentReportRepository
    {
        private AlMarsadDbContext _dbContext;

        public InitialIncidentReportRepository(AlMarsadDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task AddAsync(InitialIncidentReport report)
        {
           await _dbContext.InitialIncidentReports.AddAsync(report);  
        }

        public async Task SaveAsync()
        {
            await this._dbContext.SaveChangesAsync();  
        }

    }
}
