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

public class CityControllerTests
{
    [Fact]
    public async Task CityController_Add_ReturnsCreatedStatus()
    {
        var service = new Mock<ICityService>();
        service.Setup(x => x.AddAsync(It.IsAny<AddCityDTO>()))
            .ReturnsAsync(new ReturnCityDTO { Id = 1, ArabicName = "Arabic", EnglishName = "English" });

        var result = await new CityController(service.Object)
            .Add(new AddCityDTO { ArabicName = "Arabic", EnglishName = "English" });

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, objectResult.StatusCode);
    }

    [Fact]
    public async Task CityController_GetAll_ReturnsOkAndDelegatesSearch()
    {
        var service = new Mock<ICityService>();
        service.Setup(x => x.GetAllAsync("ram")).ReturnsAsync([new ReturnCityDTO { Id = 1 }]);

        var result = await new CityController(service.Object).GetAll("ram");

        Assert.IsType<OkObjectResult>(result);
        service.Verify(x => x.GetAllAsync("ram"), Times.Once);
    }
}
