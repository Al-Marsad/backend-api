using BL.DTO.Activity;
using BL.DTO.City;
using BL.DTO.Evidence;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.DTO.InitialIncidentReport;
using BL.DTO.LegalNote;
using BL.DTO.Question;
using BL.DTO.User;
using BL.Services.Interfaces;
using DAL.Enums;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;
using TestProject.Support;

namespace TestProject.Controllers;

public class IncidentControllerTests
{
    [Fact]
    public async Task IncidentController_AddIncident_SetsFieldResearcherFromClaims()
    {
        var service = new Mock<IIncidentService>();
        AddIncidentDTO? captured = null;
        service.Setup(x => x.AddAsync(It.IsAny<AddIncidentDTO>()))
            .Callback<AddIncidentDTO>(dto => captured = dto)
            .ReturnsAsync(new ReturnFullIncidentDTO { Id = 4, PersonalVictimTestimonies = [] });

        var controller = new IncidentController(service.Object, Mock.Of<ILegalNoteService>());
        controller.SetUser("researcher-1");

        var result = await controller.AddIncident(new AddIncidentDTO());

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
        Assert.Equal("researcher-1", captured?.FieldResearcherId);
    }

    [Fact]
    public async Task IncidentController_GetAllIncidents_UsesCityClaimWhenCityIdMissing()
    {
        var service = new Mock<IIncidentService>();
        service.Setup(x => x.GetAllIncidentsByPageAsync(It.IsAny<PaginationDTO>(), 7, false, null, null, null))
            .ReturnsAsync(new PagedResultDTO<List<ReturnIncidentDTO>> { Data = [], Page = 1, PageSize = 20 });

        var controller = new IncidentController(service.Object, Mock.Of<ILegalNoteService>());
        controller.SetUser("manager-1", cityId: 7);

        var result = await controller.GetAllIncidentsByPage(new PaginationDTO());

        Assert.IsType<OkObjectResult>(result);
        service.Verify(x => x.GetAllIncidentsByPageAsync(It.IsAny<PaginationDTO>(), 7, false, null, null, null), Times.Once);
    }

    [Fact]
    public async Task IncidentController_AssignToLegalTeamMember_WithoutUser_ReturnsUnauthorized()
    {
        var controller = new IncidentController(Mock.Of<IIncidentService>(), Mock.Of<ILegalNoteService>());
        controller.SetUser(userId: null);

        var result = await controller.AssignToLegalTeamMember(1);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
