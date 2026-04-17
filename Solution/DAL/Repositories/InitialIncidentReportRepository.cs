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
            var report = await _dbContext.InitialIncidentReports.Include(r => r.City).SingleOrDefaultAsync(r => r.Id == id);
            return report;
        }

        public async Task<List<InitialIncidentReport>> GetPageAsync(
          int skip,
          int take,
          string? userId = null,
          InitialIncidentReportStatus? status = null,
          int? cityId = null)
        {
            if (skip < 0 || take < 0)
                return new List<InitialIncidentReport>();

            var query = _dbContext.InitialIncidentReports
                .Include(i => i.City)
                .AsQueryable();

            if (userId != null)
                query = query.Where(r => r.CitizenReporterId == userId);

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            if (cityId.HasValue)
                query = query.Where(r => r.CityId == cityId.Value);

            return await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
