using DAL.DBContext;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<InitialIncidentReport?> GetByIdAsync(int id)
        {
            var report = await _dbContext.InitialIncidentReports.SingleOrDefaultAsync(r => r.Id == id);
            return report;
        }

        public async Task<List<InitialIncidentReport>> GetPageAsync(int Skip, int Take, int userId)
        {
            if(Skip < 0 || Take < 0)
            {
                return new List<InitialIncidentReport>();
            }

            return await _dbContext.InitialIncidentReports.Where(r => r.Id == userId)
                .Skip(Skip)
                .Take(Take)
                .ToListAsync();
        }

        public async Task<List<InitialIncidentReport>> GetByStatusAsync(InitialIncidentReportStatus status)
        {
            if (!Enum.IsDefined(typeof(InitialIncidentReportStatus), status))
            {
                throw new BadRequestException("Status value is unknown");
            }

            return await _dbContext.InitialIncidentReports.Where(r => r.Status == status).ToListAsync();
        }
    }
}
