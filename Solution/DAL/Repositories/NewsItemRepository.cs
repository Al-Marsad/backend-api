using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class NewsItemRepository : INewsItemRepository
    {
        private readonly AlMarsadDbContext _dbContext;

        public NewsItemRepository(AlMarsadDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(NewsItem obj)
        {
            await _dbContext.News.AddAsync(obj);
        }

        public async Task<NewsItem?> GetByIdAsync(int id)
        {
            return await IncludeDetails(_dbContext.News)
                .SingleOrDefaultAsync(n => n.Id == id);
        }

        public async Task<NewsItem?> GetByIncidentIdAsync(int incidentId)
        {
            return await IncludeDetails(_dbContext.News)
                .SingleOrDefaultAsync(n => n.IncidentId == incidentId);
        }

        public async Task<(List<NewsItem>, int)> GetByPageAsync(int skip, int take, string? searchTerm,
            string? writtenById, int? cityId , bool? isPublished)
        {
            if (skip < 0 || take < 0)
                return (new List<NewsItem>(), 0);

            var query = IncludeDetails(_dbContext.News);

            if (!string.IsNullOrWhiteSpace(writtenById))
            {
                query = query.Where(n => n.WrittenById == writtenById);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.Trim();
                query = query.Where(n =>
                    n.Title.Contains(normalizedSearch) ||
                    n.Summary.Contains(normalizedSearch) ||
                    n.Body.Contains(normalizedSearch)
                );
            }

            if (cityId != null)
            {
                query = query.Where(n => n.Incident.CityId == cityId);
            }

            if (isPublished != null)
            {
                query = query.Where(n => n.IsPublished == isPublished.Value);
            }

            var count = await query.CountAsync();

            return (await query
                .OrderByDescending(n => n.IsPublished ? (n.PublishDate ?? n.WritingDate) : n.WritingDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync(), count);
        }

        public void Update(NewsItem entity)
        {
            _dbContext.News.Update(entity);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        private static IQueryable<NewsItem> IncludeDetails(IQueryable<NewsItem> query)
        {
            return query
                .Include(n => n.Incident)
                .Include(n => n.WrittenBy);
        }
    }
}
