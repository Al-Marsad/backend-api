using BL.Services;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Moq;

namespace TestProject.Services;

public class StatsServiceTests
{
    [Fact]
    public async Task StatsService_GetPublicStatsAsync_ReturnsRepositoryCounts()
    {
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetPublicStatsAsync())
            .ReturnsAsync(new PublicStatsModel
            {
                TotalIncidents = 1,
                RegionsAffected = 2,
                ReportsThisMonth = 3,
                PendingReview = 4
            });
        var service = new StatsService(incidentRepo.Object);

        var result = await service.GetPublicStatsAsync();

        Assert.Equal(1, result.TotalIncidents);
        Assert.Equal(2, result.RegionsAffected);
        Assert.Equal(3, result.ReportsThisMonth);
        Assert.Equal(4, result.PendingReview);
    }

    [Fact]
    public async Task StatsService_GetAnalyticsAsync_ReturnsRepositoryGroups()
    {
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetAnalyticsAsync())
            .ReturnsAsync(new AnalyticsModel
            {
                ByMonth = [new CountByMonthModel { Year = 2026, Month = 6, Count = 1 }],
                ByYear = [new CountByYearModel { Year = 2026, Count = 2 }],
                ByCity = [new CountByCityModel { CityId = 3, ArabicName = "AR", EnglishName = "EN", Count = 4 }]
            });
        var service = new StatsService(incidentRepo.Object);

        var result = await service.GetAnalyticsAsync();

        Assert.Single(result.ByMonth);
        Assert.Equal(1, result.ByMonth[0].Count);
        Assert.Single(result.ByYear);
        Assert.Equal(2, result.ByYear[0].Count);
        Assert.Single(result.ByCity);
        Assert.Equal("EN", result.ByCity[0].EnglishName);
    }
}
