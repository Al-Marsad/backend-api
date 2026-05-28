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

public class TestControllerTests
{
    [Fact]
    public async Task TestController_AddAsync_WhenIdIsOne_ThrowsTestException()
    {
        await Assert.ThrowsAsync<Exception>(() => new TestController().AddAsync(1));
    }

    [Fact]
    public async Task TestController_AddAsync_WhenIdIsNotOne_ReturnsOk()
    {
        var result = await new TestController().AddAsync(2);

        Assert.IsType<OkResult>(result);
    }
}
