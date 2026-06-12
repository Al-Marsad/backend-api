using AutoMapper;
using BL.DTO.Activity;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepositoy _activityRepositoy;
        private readonly IMapper _mapper;
        public ActivityService(IActivityRepositoy activityRepositoy, IMapper mapper)
        {
            _activityRepositoy = activityRepositoy;
            _mapper = mapper;
        }

        public async Task<PagedResultDTO<List<ReturnActivityDTO>>> GetByPageAsync(PaginationDTO pageDTO,
            string? searchContent = null, ActivityType? activityType = null)
        {

            if (activityType != null)
            {
                if (!Enum.IsDefined(typeof(ActivityType), activityType))
                    throw new ValidationException($"Invalid evidence type: {activityType}");
            }

            var (activities, totalItems) = await _activityRepositoy.GetByPageAsync((pageDTO.Page - 1) * pageDTO.PageSize,
              pageDTO.PageSize, searchContent, activityType);

            var activityDTOs = _mapper.Map<List<ReturnActivityDTO>>(activities);

            return new PagedResultDTO<List<ReturnActivityDTO>>()
            {
                Data = activityDTOs,
                Page = pageDTO.Page,
                PageSize = pageDTO.PageSize,
                TotalCount = totalItems
            };
        }

        public async Task<ActivityStatsDTO> GetStatsAsync()
        {
            var stats = await _activityRepositoy.GetStatsAsync();

            return new ActivityStatsDTO
            {
                IncidentCreatedCount = stats.IncidentCreatedCount,
                IncidentUpdatedCount = stats.IncidentUpdatedCount,
                IncidentDeletedCount = stats.IncidentDeletedCount,
                RequestChangeCount = stats.RequestChangeCount,
                OtherCount = stats.OtherCount
            };
        }
    }
}
