using BL.DTO.Activity;
using BL.DTO.General;
using BL.DTO.Incident;
using DAL.Enums;

namespace BL.Services.Interfaces
{
    public interface IActivityService
    {
        public Task<PagedResultDTO<List<ReturnActivityDTO>>> GetByPageAsync(PaginationDTO pageDTO, 
            string? searchContent = null, ActivityType? activityType = null);
        public Task<ActivityStatsDTO> GetStatsAsync();
    }
}
