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

public class AuthenticationServiceTests
{
    [Fact]
    public async Task AuthenticationService_Login_WhenEmailDoesNotExist_ThrowsUnauthorizedException()
    {
        var userManager = IdentityMockFactory.CreateUserManager();
        userManager.Setup(x => x.FindByEmailAsync("missing@test.com")).ReturnsAsync((AppUser?)null);

        var service = ServiceTestFactory.CreateAuthenticationService(userManager);

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            service.Login(new LoginUserDTO { Email = "missing@test.com", Password = "P@ssw0rd!" }));
    }
}
