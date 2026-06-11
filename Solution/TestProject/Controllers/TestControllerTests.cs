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
using BL.Queue.Interfaces;

namespace TestProject.Controllers;

public class TestControllerTests
{
    [Fact]
    public async Task TestController_AddAsync_EnqueuesIncidentId()
    {
        var queue = new Mock<IIncidentClassificationQueue>();
        var controller = new TestController(queue.Object);

        await controller.AddAsync(1);

        queue.Verify(x => x.Enqueue("1"), Times.Once);
    }

    [Fact]
    public async Task TestController_AddAsync_ReturnsOk()
    {
        var controller = new TestController(Mock.Of<IIncidentClassificationQueue>());

        var result = await controller.AddAsync(2);

        Assert.IsType<OkObjectResult>(result);
    }
}
