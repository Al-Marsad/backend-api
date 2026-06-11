using BL.DTO.Stats;

namespace BL.Services.Interfaces
{
    public interface IStatsService
    {
        public Task<PublicStatsDTO> GetPublicStatsAsync();
        public Task<AnalyticsDTO> GetAnalyticsAsync();
    }
}
