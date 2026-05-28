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

public class InitialIncidentReportControllerTests
{
    [Fact]
    public async Task InitialIncidentReportController_SendReport_SetsCitizenReporterAndReturnsCreated()
    {
        var service = new Mock<IInitialIncidentReportService>();
        AddInitialIncidentReportDTO? captured = null;
        service.Setup(x => x.AddAsync(It.IsAny<AddInitialIncidentReportDTO>()))
            .Callback<AddInitialIncidentReportDTO>(dto => captured = dto)
            .ReturnsAsync(new ReturnInitialIncidentReportDTO { Id = 9 });

        var controller = new InitialIncidentReportController(service.Object);
        controller.SetUser("citizen-1");

        var result = await controller.SendReport(new AddInitialIncidentReportDTO());

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("citizen-1", captured?.CitizenReporterId);
    }

    [Fact]
    public async Task InitialIncidentReportController_AssignToFieldResearcher_BuildsAssignmentDto()
    {
        var service = new Mock<IInitialIncidentReportService>();
        AssignToFieldResearcherDTO? captured = null;
        service.Setup(x => x.AssignToFieldResearcher(It.IsAny<AssignToFieldResearcherDTO>()))
            .Callback<AssignToFieldResearcherDTO>(dto => captured = dto)
            .ReturnsAsync(new ReturnInitialIncidentReportDTO { Id = 3 });

        var controller = new InitialIncidentReportController(service.Object);
        controller.SetUser("researcher-1");

        var result = await controller.AssignToFieldResearcher(3);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(3, captured?.ReportId);
        Assert.Equal("researcher-1", captured?.FieldResearcherId);
    }
}
