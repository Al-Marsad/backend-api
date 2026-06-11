using BL.DTO.Stats;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;

namespace TestProject.Controllers;

public class StatsControllerTests
{
    [Fact]
    public async Task StatsController_GetPublicStats_ReturnsServiceData()
    {
        var service = new Mock<IStatsService>();
        service.Setup(x => x.GetPublicStatsAsync())
            .ReturnsAsync(new PublicStatsDTO { TotalIncidents = 3 });
        var controller = new StatsController(service.Object);

        var result = await controller.GetPublicStats();

        Assert.IsType<OkObjectResult>(result);
        service.Verify(x => x.GetPublicStatsAsync(), Times.Once);
    }

    [Fact]
    public async Task StatsController_GetAnalytics_ReturnsServiceData()
    {
        var service = new Mock<IStatsService>();
        service.Setup(x => x.GetAnalyticsAsync())
            .ReturnsAsync(new AnalyticsDTO
            {
                ByYear = [new CountByYearDTO { Year = 2026, Count = 1 }]
            });
        var controller = new StatsController(service.Object);

        var result = await controller.GetAnalytics();

        Assert.IsType<OkObjectResult>(result);
        service.Verify(x => x.GetAnalyticsAsync(), Times.Once);
    }
}
