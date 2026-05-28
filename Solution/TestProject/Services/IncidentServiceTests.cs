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

public class IncidentServiceTests
{
    [Fact]
    public async Task IncidentService_GetByIdAsync_WhenIncidentMissing_ThrowsDataNotFoundException()
    {
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(404)).ReturnsAsync((Incident?)null);
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo);

        await Assert.ThrowsAsync<DataNotFoundException>(() => service.GetByIdAsync(404));
    }

    [Fact]
    public async Task IncidentService_AssignToLegalTeamMember_SetsLegalTeamMemberAndSaves()
    {
        var incident = new Incident
        {
            Id = 2,
            FieldResearcherId = "researcher-1",
            DetailedDescription = "details",
            AreaName = "area"
        };
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(incident);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo, activityRepo: activityRepo);

        var result = await service.AssignToLegalTeamMember("legal-1", 2);

        Assert.Equal("legal-1", incident.LegalTeamMemberId);
        Assert.Equal("legal-1", result.LegalTeamMemberId);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Update)), Times.Once);
        incidentRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task IncidentService_UnassignToLegalTeamMember_WhenValid_ClearsLegalTeamMemberAndSaves()
    {
        var incident = CreateIncident();
        incident.LegalTeamMemberId = "legal-1";
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(incident);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo, activityRepo: activityRepo);

        var result = await service.UnassignToLegalTeamMember("legal-1", 2);

        Assert.Null(incident.LegalTeamMemberId);
        Assert.Null(result.LegalTeamMemberId);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Update)), Times.Once);
        incidentRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task IncidentService_AddAsync_WithAssignedInitialReport_CreatesIncidentUpdatesInitialReportAndLogsActivity()
    {
        var initialReport = new InitialIncidentReport
        {
            Id = 5,
            CitizenReporterId = "citizen-1",
            FieldResearcherId = "researcher-1",
            InitialDescription = "initial",
            Status = InitialIncidentReportStatus.ASSIGNED
        };
        Incident? capturedIncident = null;
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.AddAsync(It.IsAny<Incident>()))
            .Callback<Incident>(incident =>
            {
                incident.Id = 10;
                capturedIncident = incident;
            })
            .Returns(Task.CompletedTask);
        incidentRepo.Setup(x => x.GetFullByIdAsync(10)).ReturnsAsync(() => capturedIncident);
        var initialReportRepo = new Mock<IInitialIncidentReportRepository>();
        initialReportRepo.Setup(x => x.GetByIdAsync(5)).ReturnsAsync(initialReport);
        initialReportRepo.Setup(x => x.HasIncident(5)).ReturnsAsync(false);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo, initialReportRepo: initialReportRepo, activityRepo: activityRepo);

        var result = await service.AddAsync(CreateAddIncidentDto());

        Assert.Equal(10, result.Id);
        Assert.Equal(InitialIncidentReportStatus.PENDING, initialReport.Status);
        incidentRepo.Verify(x => x.AddAsync(It.Is<Incident>(i => i.FieldResearcherId == "researcher-1" && i.InitialIncidentReportId == 5)), 
            Times.Once);
        incidentRepo.Verify(x => x.SaveAsync(), Times.Once);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Add)), Times.Once);
        activityRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task IncidentService_AddAsync_WhenInitialReportAlreadyHasIncident_ThrowsConflict()
    {
        var initialReportRepo = new Mock<IInitialIncidentReportRepository>();
        initialReportRepo.Setup(x => x.GetByIdAsync(5)).ReturnsAsync(new InitialIncidentReport
        {
            Id = 5,
            CitizenReporterId = "citizen-1",
            FieldResearcherId = "researcher-1",
            InitialDescription = "initial",
            Status = InitialIncidentReportStatus.ASSIGNED
        });
        initialReportRepo.Setup(x => x.HasIncident(5)).ReturnsAsync(true);
        var service = ServiceTestFactory.CreateIncidentService(new Mock<IIncidentRepository>(), initialReportRepo: initialReportRepo);

        await Assert.ThrowsAsync<ConflictException>(() => service.AddAsync(CreateAddIncidentDto()));
    }

    [Fact]
    public async Task IncidentService_UpdateIncident_WhenAssignedToCurrentUser_UpdatesFieldsLocksAndSaves()
    {
        var incident = CreateIncident();
        incident.LegalTeamMemberId = "legal-1";
        incident.PersonalVictimTestimonies =
        [
            new PersonalVictimTestimonie { Id = 3, VictimId = 4, IncidentId = 2, PersonalNarrative = "old" }
        ];
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetWithTestimoniesOnlyById(2)).ReturnsAsync(incident);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo, activityRepo: activityRepo);

        var result = await service.UpdateIncident(new UpdateIncidentDTO
        {
            DetailedDescription = "updated details",
            AIClassification = "classification",
            PerpetratorDescription = "updated perpetrator",
            Testimonies = [new BL.DTO.Victim.UpdateTestimonyDTO { Id = 3, PersonalNarrative = "new" }]
        }, 2, "legal-1");

        Assert.Equal("updated details", incident.DetailedDescription);
        Assert.Equal("classification", incident.AIClassification);
        Assert.True(incident.PreventModification);
        Assert.Equal("new", incident.PersonalVictimTestimonies[0].PersonalNarrative);
        Assert.True(result.PreventModification);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Update)), Times.Once);
        incidentRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task IncidentService_GetAllIncidentsByPage_RejectsOutOfRangeSensitivity()
    {
        var service = ServiceTestFactory.CreateIncidentService(new Mock<IIncidentRepository>());

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.GetAllIncidentsByPageAsync(new PaginationDTO(), null, false, null, null, 11));
    }

    [Fact]
    public async Task IncidentService_AddRangeOfRelatedEvidences_WhenEmpty_ThrowsValidationException()
    {
        var service = ServiceTestFactory.CreateIncidentService(new Mock<IIncidentRepository>());

        await Assert.ThrowsAsync<ValidationException>(() => service.AddRangeOfRelatedEvidences([], 1));
    }

    [Fact]
    public async Task IncidentService_GetEvidencesByIncidentIdAsync_WhenIncidentMissing_ThrowsDataNotFoundException()
    {
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Incident?)null);
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo);

        await Assert.ThrowsAsync<DataNotFoundException>(() => service.GetEvidencesByIncidentIdAsync(99));
    }

    [Fact]
    public async Task IncidentService_GetTestimoniesAndTheirVictimsByIncidentIdAsync_WhenIncidentExists_ReturnsMappedTestimonies()
    {
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(CreateIncident());
        incidentRepo.Setup(x => x.GetTestimoniesAndTheirVictimsByIncidentIdAsync(2))
            .ReturnsAsync(
            [
                new PersonalVictimTestimonie
                {
                    Id = 7,
                    IncidentId = 2,
                    VictimId = 8,
                    InjuryStatus = InjuryStatus.Injured,
                    Victim = CreateVictim()
                }
            ]);
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo);

        var result = await service.GetTestimoniesAndTheirVictimsByIncidentIdAsync(2);

        Assert.Single(result);
        Assert.Equal(7, result[0].Id);
        Assert.Equal("123", result[0].Victim.NationalId);
    }

    [Fact]
    public async Task IncidentService_GiveDocumentationConsentAsync_WhenValid_ClosesInitialReportLocksIncidentAndSaves()
    {
        var incident = CreateIncident();
        incident.LegalTeamMemberId = "legal-1";
        incident.InitialIncidentReportId = 5;
        var initialReport = new InitialIncidentReport { Id = 5, InitialDescription = "initial", CitizenReporterId = "citizen-1" };
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(incident);
        var initialReportRepo = new Mock<IInitialIncidentReportRepository>();
        initialReportRepo.Setup(x => x.GetByIdAsync(5)).ReturnsAsync(initialReport);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo, initialReportRepo: initialReportRepo, activityRepo: activityRepo);

        var result = await service.GiveDocumentationConsentAsync(2, "legal-1");

        Assert.True(incident.DocumentationConsent);
        Assert.True(incident.PreventModification);
        Assert.Equal(InitialIncidentReportStatus.CLOSED, initialReport.Status);
        Assert.True(result.DocumentationConsent);
        incidentRepo.Verify(x => x.SaveAsync(), Times.Once);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Update)), Times.Once);
    }

    [Fact]
    public async Task IncidentService_RequestModification_WhenIncidentNotLocked_ThrowsConflict()
    {
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Incident
        {
            Id = 1,
            LegalTeamMemberId = "legal-1",
            PreventModification = false,
            FieldResearcherId = "researcher-1",
            DetailedDescription = "details",
            AreaName = "area"
        });
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo);

        await Assert.ThrowsAsync<ConflictException>(() => service.RequestModificationAsync(1, "legal-1"));
    }

    [Fact]
    public async Task IncidentService_RequestModification_WhenValid_LogsActivityAndSavesActivity()
    {
        var incident = CreateIncident();
        incident.LegalTeamMemberId = "legal-1";
        incident.PreventModification = true;
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(incident);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo, activityRepo: activityRepo);

        await service.RequestModificationAsync(2, "legal-1");

        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.RequestChange)), Times.Once);
        activityRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task IncidentService_AllowModificationAsync_WhenValid_UnlocksIncidentReopensInitialReportAndSaves()
    {
        var incident = CreateIncident();
        incident.PreventModification = true;
        incident.DocumentationConsent = true;
        incident.InitialIncidentReportId = 5;
        var initialReport = new InitialIncidentReport
        {
            Id = 5,
            InitialDescription = "initial",
            CitizenReporterId = "citizen-1",
            Status = InitialIncidentReportStatus.CLOSED
        };
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(incident);
        var initialReportRepo = new Mock<IInitialIncidentReportRepository>();
        initialReportRepo.Setup(x => x.GetByIdAsync(5)).ReturnsAsync(initialReport);
        var activityRepo = new Mock<IActivityRepositoy>();
        var service = ServiceTestFactory.CreateIncidentService(incidentRepo, initialReportRepo: initialReportRepo, activityRepo: activityRepo);

        var result = await service.AllowModificationAsync(2, "manager-1");

        Assert.False(incident.PreventModification);
        Assert.False(incident.DocumentationConsent);
        Assert.Equal(InitialIncidentReportStatus.PENDING, initialReport.Status);
        Assert.False(result.PreventModification);
        incidentRepo.Verify(x => x.SaveAsync(), Times.Once);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Update)), Times.Once);
    }

    private static Incident CreateIncident()
    {
        return new Incident
        {
            Id = 2,
            FieldResearcherId = "researcher-1",
            DetailedDescription = "details",
            AreaName = "area",
            AreaClass = AreaClass.A,
            AreaType = AreaType.Residential,
            DateOfOccurrence = DateTime.UtcNow.AddDays(-1),
            CityId = 1,
            SensitivityScore = 5
        };
    }

    private static AddIncidentDTO CreateAddIncidentDto()
    {
        return new AddIncidentDTO
        {
            DateOfOccurrence = DateTime.UtcNow.AddDays(-1),
            DetailedDescription = "details",
            AreaName = "area",
            AreaClass = AreaClass.A,
            AreaType = AreaType.Residential,
            LocationLat = 31,
            LocationLng = 35,
            SensitivityScore = 5,
            CityId = 1,
            InitialIncidentReportId = 5,
            FieldResearcherId = "researcher-1"
        };
    }

    private static Victim CreateVictim()
    {
        return new Victim
        {
            Id = 8,
            FirstName = "First",
            SecondName = "Second",
            ThirdName = "Third",
            LastName = "Last",
            NationalId = "123",
            PhoneNumber = "0590000000"
        };
    }
}
