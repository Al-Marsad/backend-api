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

public class AuthControllerTests
{
    [Fact]
    public async Task AuthController_Register_WithoutRole_ReturnsUnprocessableEntity()
    {
        var controller = new AuthController(Mock.Of<IAuthenticationService>());

        var result = await controller.Register(new AddUserDTO());

        Assert.IsType<UnprocessableEntityObjectResult>(result);
    }

    [Fact]
    public async Task AuthController_Login_AppendsRefreshCookieAndReturnsOk()
    {
        var auth = new Mock<IAuthenticationService>();
        auth.Setup(x => x.Login(It.IsAny<LoginUserDTO>()))
            .ReturnsAsync(new ReturnLoginUserDTO
            {
                Id = "u1",
                RefreshToken = "refresh-token",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1)
            });

        var controller = new AuthController(auth.Object);
        controller.SetHttpContext();

        var result = await controller.Login(new LoginUserDTO { Email = "a@test.com", Password = "P@ssw0rd!" });

        Assert.IsType<OkObjectResult>(result);
        Assert.Contains("RefreshToken=", controller.Response.Headers.SetCookie.ToString());
    }

    [Fact]
    public async Task AuthController_Refresh_UsesBodyTokenWhenProvided()
    {
        var auth = new Mock<IAuthenticationService>();
        auth.Setup(x => x.Refresh("body-token"))
            .ReturnsAsync(new ReturnAccessTokenDTO
            {
                AccessToken = "access",
                RefreshToken = "new-refresh",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1)
            });

        var controller = new AuthController(auth.Object);
        controller.SetHttpContext();

        var result = await controller.Refresh(new RefreshTokenDTO { RefreshToken = "body-token" });

        Assert.IsType<OkObjectResult>(result);
        auth.Verify(x => x.Refresh("body-token"), Times.Once);
    }

    [Fact]
    public async Task AuthController_Logout_WithoutToken_ThrowsUnauthorizedException()
    {
        var controller = new AuthController(Mock.Of<IAuthenticationService>());
        controller.SetHttpContext();

        await Assert.ThrowsAsync<UnauthorizedException>(() => controller.Logout());
    }
}
