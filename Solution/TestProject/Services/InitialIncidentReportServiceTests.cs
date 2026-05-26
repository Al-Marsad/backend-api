using System.IdentityModel.Tokens.Jwt;
using BL.DTO.City;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.DTO.InitialIncidentReport;
using BL.DTO.LegalNote;
using BL.DTO.Question;
using BL.DTO.User;
using BL.Helper;
using BL.Services;
using BL.Services.Interfaces;
using DAL.DBContext;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using TestProject.Support;
using ValidationException = DAL.Exceptions.ValidationException;

namespace TestProject.Services;

public class InitialIncidentReportServiceTests
{
    [Fact]
    public async Task InitialIncidentReportService_GetById_WhenReportBelongsToAnotherCitizen_ThrowsForbidden()
    {
        var repo = new Mock<IInitialIncidentReportRepository>();
        repo.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new InitialIncidentReport { Id = 1, CitizenReporterId = "citizen-1", InitialDescription = "desc" });

        var service = new InitialIncidentReportService(repo.Object, TestMapper.Create(), Mock.Of<IActivityRepositoy>());

        await Assert.ThrowsAsync<ForbiddenException>(() => service.GetByIdAsync(1, "citizen-2"));
    }

    [Fact]
    public async Task InitialIncidentReportService_AssignToFieldResearcher_UpdatesStatusAndSaves()
    {
        var report = new InitialIncidentReport { Id = 1, CitizenReporterId = "citizen-1", InitialDescription = "desc" };
        var repo = new Mock<IInitialIncidentReportRepository>();
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(report);
        var activityRepo = new Mock<IActivityRepositoy>();

        var service = new InitialIncidentReportService(repo.Object, TestMapper.Create(), activityRepo.Object);

        var result = await service.AssignToFieldResearcher(new AssignToFieldResearcherDTO
        {
            ReportId = 1,
            FieldResearcherId = "researcher-1"
        });

        Assert.Equal(InitialIncidentReportStatus.ASSIGNED, report.Status);
        Assert.Equal("researcher-1", report.FieldResearcherId);
        Assert.Equal("researcher-1", result.FieldResearcherId);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Update)), Times.Once);
        repo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InitialIncidentReportService_GetByPage_ForFieldResearcherDefaultsCityFromCurrentUser()
    {
        var repo = new Mock<IInitialIncidentReportRepository>();
        repo.Setup(x => x.GetPageAsync(0, 20, null, null, 8))
            .ReturnsAsync((new List<InitialIncidentReport>(), 0));

        var service = new InitialIncidentReportService(repo.Object, TestMapper.Create(), Mock.Of<IActivityRepositoy>());

        await service.GetByPageAsync(new GetByPageInitialIncidentReportDTO(), new CurrentUser
        {
            Role = RolesSelector.FieldResearcher,
            CityId = "8"
        });

        repo.Verify(x => x.GetPageAsync(0, 20, null, null, 8), Times.Once);
    }
}
