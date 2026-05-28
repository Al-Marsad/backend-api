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

public class UserControllerTests
{
    [Fact]
    public async Task UserController_GetProfile_WithoutUser_ReturnsUnauthorized()
    {
        var controller = new UserController(Mock.Of<IUserService>());
        controller.SetUser(userId: null);

        var result = await controller.GetProfile();

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task UserController_GetProfile_UsesCurrentUser()
    {
        var service = new Mock<IUserService>();
        service.Setup(x => x.GetProfileAsync("user-1")).ReturnsAsync(new GetUserPorfileDTO { Id = "user-1" });
        var controller = new UserController(service.Object);
        controller.SetUser("user-1");

        var result = await controller.GetProfile();

        Assert.IsType<OkObjectResult>(result);
        service.Verify(x => x.GetProfileAsync("user-1"), Times.Once);
    }

    [Fact]
    public async Task UserController_ChangeAccountStatus_WithoutRouteUserId_ReturnsBadRequest()
    {
        var result = await new UserController(Mock.Of<IUserService>())
            .ChangeAccountStatus("", new ChangeAccountStatusDTO { Status = AccountStatus.Active });

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
