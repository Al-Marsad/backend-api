using AutoMapper;
using BL.DTO.General;
using BL.DTO.News;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class NewsItemService : INewsItemService
    {
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IActivityRepositoy _activityRepositoy;
        private readonly IMapper _mapper;

        public NewsItemService(INewsItemRepository newsItemRepository,
            IIncidentRepository incidentRepository,
            IActivityRepositoy activityRepositoy,
            IMapper mapper)
        {
            _newsItemRepository = newsItemRepository;
            _incidentRepository = incidentRepository;
            _activityRepositoy = activityRepositoy;
            _mapper = mapper;
        }

        public async Task<ReturnTinyNewsItemDTO> AddAsync(AddNewsItemDTO newsDTO, CurrentUser currentUser)
        {

            var incident = await _incidentRepository.GetByIdAsync(newsDTO.IncidentId.Value);
            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{newsDTO.IncidentId}' not found");


            if (!incident.DocumentationConsent)
            {
                throw new ConflictException($"Incident with id '{incident.Id}' must have documentation consent before writing news item");
            }

            if (!incident.PublicationConsent)
            {
                throw new ConflictException($"Incident with id '{incident.Id}' must have publication consent before writing news item");
            }

            var existingNews = await _newsItemRepository.GetByIncidentIdAsync(newsDTO.IncidentId.Value);
            if (existingNews != null)
            {
                throw new ConflictException($"Incident with id '{newsDTO.IncidentId}' already has news item with id '{existingNews.Id}'");
            }

            var news = _mapper.Map<NewsItem>(newsDTO);
            news.WrittenById = currentUser.UserId;
            news.WritingDate = DateTime.UtcNow;
            news.IsPublished = false;
            news.PublishDate = null;

            await _newsItemRepository.AddAsync(news);
            await _newsItemRepository.SaveAsync();

            await AddActivityAsync(currentUser.UserId,
                $"Manager with id '{currentUser.UserId}' added draft news item with id '{news.Id}' for incident with id '{incident.Id}'",
                ActivityType.Add);

            var loadedNews = await _newsItemRepository.GetByIdAsync(news.Id);
            return _mapper.Map<ReturnTinyNewsItemDTO>(loadedNews);
        }

        public async Task<ReturnUpdatedNewsItemDTO> UpdateAsync(int newsId, UpdateNewsItemDTO newsDTO, CurrentUser currentUser)
        {
            var news = await GetExistingNewsAsync(newsId);

            news.Title = newsDTO.Title ?? news.Title;
            news.Body = newsDTO.Body ?? news.Body;
            news.Summary = newsDTO.Summary ?? news.Summary;
            news.ImageUrl = newsDTO.ImageUrl ?? news.ImageUrl;

            await _newsItemRepository.SaveAsync();

            await AddActivityAsync(currentUser.UserId,
                $"Manager with id '{currentUser.UserId}' updated news item with id '{news.Id}'",
                ActivityType.Update);

            return _mapper.Map<ReturnUpdatedNewsItemDTO>(news);
        }

        public async Task<ReturnTinyNewsItemDTO> PublishAsync(int newsId, CurrentUser currentUser)
        {
            var news = await GetExistingNewsAsync(newsId);
            if (news.IsPublished)
                throw new ConflictException($"News item with id '{newsId}' is already published");

            news.IsPublished = true;
            news.PublishDate = DateTime.UtcNow;

            _newsItemRepository.Update(news);
            await _newsItemRepository.SaveAsync();

            await AddActivityAsync(currentUser.UserId,
                $"Manager with id '{currentUser.UserId}' published news item with id '{news.Id}'",
                ActivityType.Update);

            return _mapper.Map<ReturnTinyNewsItemDTO>(news);
        }

        public async Task<ReturnTinyNewsItemDTO> UnpublishAsync(int newsId, CurrentUser currentUser)
        {
            var news = await GetExistingNewsAsync(newsId);
            if (!news.IsPublished)
                throw new ConflictException($"News item with id '{newsId}' is not published already");

            news.IsPublished = false;
            news.PublishDate = null;

            _newsItemRepository.Update(news);
            await _newsItemRepository.SaveAsync();

            await AddActivityAsync(currentUser.UserId,
                $"Manager with id '{currentUser.UserId}' unpublished news item with id '{news.Id}'",
                ActivityType.Update);

            return _mapper.Map<ReturnTinyNewsItemDTO>(news);
        }

        public async Task<ReturnNewsItemDTO> GetManagementByIdAsync(int newsId)
        {
            return _mapper.Map<ReturnNewsItemDTO>(await GetExistingNewsAsync(newsId));
        }

        public async Task<PagedResultDTO<List<ReturnTinyNewsItemDTO>>> GetManagementByPageAsync(PaginationDTO pageDTO,
            string? searchTerm = null, string? writtenById = null, int? cityId = null,
           bool? isPublished = null)
        {
            var (newsItems, totalItems) = await _newsItemRepository.GetByPageAsync((pageDTO.Page - 1) * pageDTO.PageSize,
                pageDTO.PageSize, searchTerm, writtenById, cityId, isPublished);

            return BuildPagedResult(pageDTO, _mapper.Map<List<ReturnTinyNewsItemDTO>>(newsItems), totalItems);
        }

        public async Task<PagedResultDTO<List<ReturnTinyNewsItemDTO>>> GetByWrittenByIdAsync(PaginationDTO pageDTO,
            string writtenById, string? searchTerm = null, int? cityId = null, bool? isPublished = null)
        {
            if (string.IsNullOrWhiteSpace(writtenById))
                throw new ValidationException("Validation failed", new { WrittenById = "Value is required" });

            return await GetManagementByPageAsync(pageDTO, searchTerm, writtenById, cityId, isPublished);
        }

        public async Task<PagedResultDTO<List<ReturnAbbreviatedNewsItemDTO>>> GetWebsiteAbbreviatedNewsAsync(PaginationDTO pageDTO,
            string? searchTerm = null, int? cityId = null)
        {
            var (newsItems, totalItems) = await _newsItemRepository.GetByPageAsync((pageDTO.Page - 1) * pageDTO.PageSize,
                pageDTO.PageSize, searchTerm, null, cityId, true);

            return BuildPagedResult(pageDTO, _mapper.Map<List<ReturnAbbreviatedNewsItemDTO>>(newsItems), totalItems);
        }

        public async Task<PagedResultDTO<List<ReturnNewsMapPointDTO>>> GetWebsiteMapNewsAsync(PaginationDTO pageDTO,
            string? searchTerm = null, int? cityId = null)
        {
            var (newsItems, totalItems) = await _newsItemRepository.GetByPageAsync((pageDTO.Page - 1) * pageDTO.PageSize,
                pageDTO.PageSize, searchTerm, null, cityId, true);

            return BuildPagedResult(pageDTO, _mapper.Map<List<ReturnNewsMapPointDTO>>(newsItems), totalItems);
        }

        public async Task<ReturnNewsItemDTO> GetWebsiteByIdAsync(int newsId)
        {
            var news = await GetExistingNewsAsync(newsId);
            if (!news.IsPublished)
                throw new DataNotFoundException($"News item with id '{newsId}' not published");

            return _mapper.Map<ReturnNewsItemDTO>(news);
        }

        public async Task<NewsItemStatsDTO> GetStatsAsync()
        {
            var stats = await _newsItemRepository.GetStatsAsync();

            return new NewsItemStatsDTO
            {
                TotalCount = stats.TotalCount,
                PublishedCount = stats.PublishedCount,
                HiddenCount = stats.HiddenCount
            };
        }

        private async Task<NewsItem> GetExistingNewsAsync(int newsId)
        {
            var news = await _newsItemRepository.GetByIdAsync(newsId);
            if (news == null)
                throw new DataNotFoundException($"News item with id '{newsId}' not published");

            return news;
        }

        private async Task AddActivityAsync(string userId, string description, ActivityType type)
        {
            await _activityRepositoy.AddAsync(new Activity
            {
                Description = description,
                MadeById = userId,
                Type = type
            });
            await _activityRepositoy.SaveAsync();
        }

        private static PagedResultDTO<List<T>> BuildPagedResult<T>(PaginationDTO pageDTO, List<T> items, int totalItems)
        {
            return new PagedResultDTO<List<T>>
            {
                Data = items,
                Page = pageDTO.Page,
                PageSize = pageDTO.PageSize,
                TotalCount = totalItems
            };
        }
    }
}
