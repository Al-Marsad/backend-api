using BL.DTO.Stats;
using BL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class StatsService : IStatsService
    {
        private readonly IIncidentRepository _incidentRepo;

        public StatsService(IIncidentRepository incidentRepo)
        {
            _incidentRepo = incidentRepo;
        }

        public async Task<PublicStatsDTO> GetPublicStatsAsync()
        {
            var stats = await _incidentRepo.GetPublicStatsAsync();

            return new PublicStatsDTO
            {
                TotalIncidents = stats.TotalIncidents,
                RegionsAffected = stats.RegionsAffected,
                ReportsThisMonth = stats.ReportsThisMonth,
                PendingReview = stats.PendingReview
            };
        }

        public async Task<AnalyticsDTO> GetAnalyticsAsync()
        {
            var analytics = await _incidentRepo.GetAnalyticsAsync();

            return new AnalyticsDTO
            {
                ByMonth = analytics.ByMonth.Select(item => new CountByMonthDTO
                {
                    Year = item.Year,
                    Month = item.Month,
                    Count = item.Count
                }).ToList(),
                ByYear = analytics.ByYear.Select(item => new CountByYearDTO
                {
                    Year = item.Year,
                    Count = item.Count
                }).ToList(),
                ByCity = analytics.ByCity.Select(item => new CountByCityDTO
                {
                    CityId = item.CityId,
                    ArabicName = item.ArabicName,
                    EnglishName = item.EnglishName,
                    Count = item.Count
                }).ToList()
            };
        }
    }
}
