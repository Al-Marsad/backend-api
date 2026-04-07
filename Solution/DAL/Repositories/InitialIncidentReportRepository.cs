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

        public async Task<List<InitialIncidentReport>> GetPageAsync(int Skip, int Take, string userId)
        {
            if(Skip < 0 || Take < 0)
            {
                return new List<InitialIncidentReport>();
            }

            return await _dbContext.InitialIncidentReports.Where(r => r.CitizenReporterId == userId)
                .Skip(Skip)
                .Take(Take)
                .ToListAsync();
        }

        public async Task<List<InitialIncidentReport>> GetPageAsync(int Skip, int Take, string userId, InitialIncidentReportStatus status)
        {
            if (Skip < 0 || Take < 0)
            {
                return new List<InitialIncidentReport>();
            }

            return await _dbContext.InitialIncidentReports.Where(r => r.CitizenReporterId == userId && r.Status == status)
                .Skip(Skip)
                .Take(Take)
                .ToListAsync();
        }
    }
}
