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
    public async Task InitialIncidentReportService_AddAsync_AddsReportLogsActivityAndSaves()
    {
        var repo = new Mock<IInitialIncidentReportRepository>();
        InitialIncidentReport? addedReport = null;
        repo.Setup(x => x.AddAsync(It.IsAny<InitialIncidentReport>()))
            .Callback<InitialIncidentReport>(report =>
            {
                report.Id = 11;
                addedReport = report;
            })
            .Returns(Task.CompletedTask);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = new InitialIncidentReportService(repo.Object, TestMapper.Create(), activityRepo.Object);

        var result = await service.AddAsync(new AddInitialIncidentReportDTO
        {
            InitialDescription = "initial report",
            CityId = 1,
            LocationLat = 31,
            LocationLng = 35,
            CitizenReporterId = "citizen-1"
        });

        Assert.Equal(11, result.Id);
        Assert.Equal("citizen-1", addedReport?.CitizenReporterId);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Add && a.MadeById == "citizen-1")), Times.Once);
        repo.Verify(x => x.SaveAsync(), Times.Once);
    }

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

    [Fact]
    public async Task InitialIncidentReportService_GetByPage_ForCitizenUsesCurrentUserId()
    {
        var repo = new Mock<IInitialIncidentReportRepository>();
        repo.Setup(x => x.GetPageAsync(0, 20, "citizen-1", null, null))
            .ReturnsAsync((new List<InitialIncidentReport>(), 0));
        var service = new InitialIncidentReportService(repo.Object, TestMapper.Create(), Mock.Of<IActivityRepositoy>());

        await service.GetByPageAsync(new GetByPageInitialIncidentReportDTO(), new CurrentUser
        {
            Role = RolesSelector.Citizen,
            UserId = "citizen-1"
        });

        repo.Verify(x => x.GetPageAsync(0, 20, "citizen-1", null, null), Times.Once);
    }

    [Fact]
    public async Task InitialIncidentReportService_UnassignToFieldResearcher_WhenValid_ClearsResearcherAndSaves()
    {
        var report = new InitialIncidentReport
        {
            Id = 1,
            CitizenReporterId = "citizen-1",
            FieldResearcherId = "researcher-1",
            InitialDescription = "desc",
            Status = InitialIncidentReportStatus.ASSIGNED
        };
        var repo = new Mock<IInitialIncidentReportRepository>();
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(report);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = new InitialIncidentReportService(repo.Object, TestMapper.Create(), activityRepo.Object);

        var result = await service.UnassignToFieldResearcher(new AssignToFieldResearcherDTO
        {
            ReportId = 1,
            FieldResearcherId = "researcher-1"
        });

        Assert.Null(report.FieldResearcherId);
        Assert.Equal(InitialIncidentReportStatus.UNASSIGNED, report.Status);
        Assert.Null(result.FieldResearcherId);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Update)), Times.Once);
        repo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public void InitialIncidentReportService_GetStatusValues_ReturnsAllInitialReportStatuses()
    {
        var service = new InitialIncidentReportService(Mock.Of<IInitialIncidentReportRepository>(), TestMapper.Create(), Mock.Of<IActivityRepositoy>());

        var result = service.GetStatusValues();

        Assert.Equal(Enum.GetValues<InitialIncidentReportStatus>().Length, result.Count);
        Assert.Contains(result, x => x.Status == InitialIncidentReportStatus.ASSIGNED);
    }

    [Fact]
    public async Task InitialIncidentReportService_GetMyAssignedReportsAsync_UsesPaginationAndSearch()
    {
        var repo = new Mock<IInitialIncidentReportRepository>();
        repo.Setup(x => x.GetAssignedReportsAsync("researcher-1", 20, 10, "olive"))
            .ReturnsAsync((new List<InitialIncidentReport>
            {
                new InitialIncidentReport { Id = 2, CitizenReporterId = "citizen-1", InitialDescription = "olive field" }
            }, 1));
        var service = new InitialIncidentReportService(repo.Object, TestMapper.Create(), Mock.Of<IActivityRepositoy>());

        var result = await service.GetMyAssignedReportsAsync("researcher-1", new PaginationDTO { Page = 3, PageSize = 10 }, "olive");

        Assert.Single(result.Data);
        Assert.Equal(3, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalCount);
        repo.Verify(x => x.GetAssignedReportsAsync("researcher-1", 20, 10, "olive"), Times.Once);
    }
}
