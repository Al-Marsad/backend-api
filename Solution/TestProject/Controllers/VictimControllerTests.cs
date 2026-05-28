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

public class VictimControllerTests
{
    [Fact]
    public async Task VictimController_CheckVictimExists_ReturnsOk()
    {
        var service = new Mock<IVictimService>();
        service.Setup(x => x.VictimExists("123")).ReturnsAsync(true);

        var result = await new VictimController(service.Object).CheckVictimExists("123");

        Assert.IsType<OkObjectResult>(result);
    }
}
