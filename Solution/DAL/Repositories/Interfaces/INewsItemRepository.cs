using DAL.Entities;
using DAL.Models;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface INewsItemRepository :
        ICreateRepository<NewsItem>,
        IGetByIdRepository<NewsItem>,
        IUpdateRepository<NewsItem>,
        ISaveRepository
    {
        public Task<NewsItem?> GetByIncidentIdAsync(int incidentId);
        public Task<(List<NewsItem>, int)> GetByPageAsync(int skip, int take, string? searchTerm,
            string? writtenById, int? cityId, bool? isPublished);
        public Task<NewsItemStatsModel> GetStatsAsync();
    }
}
