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

public class UserServiceTests
{
    [Fact]
    public async Task UserService_ChangePassword_WhenNewPasswordMatchesCurrent_ThrowsValidationException()
    {
        var userManager = IdentityMockFactory.CreateUserManager();
        var service = ServiceTestFactory.CreateUserService(userManager, TestDbContextFactory.Create());

        await Assert.ThrowsAsync<ValidationException>(() => service.ChangePasswordAsync(new ChangePasswordDTO
        {
            CurrentPassword = "P@ssw0rd!",
            NewPassword = "P@ssw0rd!"
        }, "user-1"));
    }

    [Fact]
    public async Task UserService_GetProfile_WhenUserMissing_ThrowsDataNotFoundException()
    {
        var userManager = IdentityMockFactory.CreateUserManager();
        userManager.Setup(x => x.FindByIdAsync("missing")).ReturnsAsync((AppUser?)null);
        var service = ServiceTestFactory.CreateUserService(userManager, TestDbContextFactory.Create());

        await Assert.ThrowsAsync<DataNotFoundException>(() => service.GetProfileAsync("missing"));
    }
}
