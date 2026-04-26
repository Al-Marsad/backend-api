using DAL.DBContext;
using DAL.Entities;
using DAL.Enums;
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

        public async Task<int> CountAsync()
        {
            return await _dbContext.InitialIncidentReports.CountAsync();
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

        public async Task<(List<InitialIncidentReport>, int TotalItems)> GetPageAsync(
          int skip,
          int take,
          string? userId = null,
          InitialIncidentReportStatus? status = null,
          int? cityId = null)
        {
            if (skip < 0 || take < 0)
                return (new List<InitialIncidentReport>(), 0);

            var query = _dbContext.InitialIncidentReports
                .Include(i => i.City)
                .AsQueryable();


            if (userId != null)
                query = query.Where(r => r.CitizenReporterId == userId);

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            if (cityId.HasValue)
                query = query.Where(r => r.CityId == cityId.Value);
            
            var totalItems = await query.CountAsync();

            var reports = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (reports, totalItems);
        }

        public async Task<(List<InitialIncidentReport>, int totalItems)> GetAssignedReportsAsync(string userId, int Skip, 
            int Take, string? search = null)
        {
            var query = _dbContext.InitialIncidentReports
                .Where(x => x.FieldResearcherId == userId &&
                            x.Status == InitialIncidentReportStatus.ASSIGNED);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.InitialDescription.Contains(search));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreationDate)
                .Skip(Skip)
                .Take(Take)
                .ToListAsync();

            return (items, totalItems);
        }
    }
}

