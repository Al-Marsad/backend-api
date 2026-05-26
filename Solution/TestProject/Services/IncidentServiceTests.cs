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
}
