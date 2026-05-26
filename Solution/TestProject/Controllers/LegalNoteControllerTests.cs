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

public class LegalNoteControllerTests
{
    [Fact]
    public async Task LegalNoteController_AddLegalNote_WithoutUser_ReturnsUnauthorized()
    {
        var controller = new LegalNoteController(Mock.Of<ILegalNoteService>());
        controller.SetUser(userId: null);

        var result = await controller.AddLegalNote(new AddLegalNoteDTO());

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task LegalNoteController_AddLegalNote_UsesCurrentUser()
    {
        var service = new Mock<ILegalNoteService>();
        service.Setup(x => x.AddAsync("legal-1", It.IsAny<AddLegalNoteDTO>()))
            .ReturnsAsync(new ReturnLegalNoteDTO { Id = 1 });

        var controller = new LegalNoteController(service.Object);
        controller.SetUser("legal-1");

        var result = await controller.AddLegalNote(new AddLegalNoteDTO { Content = "note", IncidentId = 2 });

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
        service.Verify(x => x.AddAsync("legal-1", It.IsAny<AddLegalNoteDTO>()), Times.Once);
    }
}
