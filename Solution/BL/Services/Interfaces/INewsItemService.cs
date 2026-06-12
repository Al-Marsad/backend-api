using BL.DTO.General;
using BL.DTO.News;
using BL.Helper;

namespace BL.Services.Interfaces
{
    public interface INewsItemService
    {
        public Task<ReturnTinyNewsItemDTO> AddAsync(AddNewsItemDTO newsDTO, CurrentUser currentUser);
        public Task<ReturnUpdatedNewsItemDTO> UpdateAsync(int newsId, UpdateNewsItemDTO newsDTO, CurrentUser currentUser);
        public Task<ReturnTinyNewsItemDTO> PublishAsync(int newsId, CurrentUser currentUser);
        public Task<ReturnTinyNewsItemDTO> UnpublishAsync(int newsId, CurrentUser currentUser);

        public Task<ReturnNewsItemDTO> GetManagementByIdAsync(int newsId);
        public Task<PagedResultDTO<List<ReturnTinyNewsItemDTO>>> GetManagementByPageAsync(PaginationDTO pageDTO,
            string? searchTerm = null, string? writtenById = null, int? cityId = null,
            bool? isPublished = null);
        public Task<PagedResultDTO<List<ReturnTinyNewsItemDTO>>> GetByWrittenByIdAsync(PaginationDTO pageDTO,
            string writtenById, string? searchTerm = null, int? cityId = null, bool? isPublished = null);
        public Task<PagedResultDTO<List<ReturnAbbreviatedNewsItemDTO>>> GetWebsiteAbbreviatedNewsAsync(PaginationDTO pageDTO,
            string? searchTerm = null, int? cityId = null);
        public Task<PagedResultDTO<List<ReturnNewsMapPointDTO>>> GetWebsiteMapNewsAsync(PaginationDTO pageDTO,
            string? searchTerm = null, int? cityId = null);
        public Task<ReturnNewsItemDTO> GetWebsiteByIdAsync(int newsId);
        public Task<NewsItemStatsDTO> GetStatsAsync();
    }
}
