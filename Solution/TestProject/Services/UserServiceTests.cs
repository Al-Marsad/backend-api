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
    public async Task UserService_GetProfile_WhenUserExists_ReturnsProfileWithRoles()
    {
        var user = CreateUser();
        var userManager = IdentityMockFactory.CreateUserManager();
        userManager.Setup(x => x.FindByIdAsync("user-1")).ReturnsAsync(user);
        userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync([RolesSelector.Admin]);
        var service = ServiceTestFactory.CreateUserService(userManager, TestDbContextFactory.Create());

        var result = await service.GetProfileAsync("user-1");

        Assert.Equal("user-1", result.Id);
        Assert.Equal(RolesSelector.Admin, result.Role);
    }

    [Fact]
    public async Task UserService_UpdateProfileAsync_WhenUserExists_UpdatesProfileAndReturnsDto()
    {
        var user = CreateUser();
        var userManager = IdentityMockFactory.CreateUserManager();
        userManager.Setup(x => x.FindByIdAsync("user-1")).ReturnsAsync(user);
        userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
        userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync([RolesSelector.Citizen]);
        var service = ServiceTestFactory.CreateUserService(userManager, TestDbContextFactory.Create());

        var result = await service.UpdateProfileAsync(new UpdateUserProfileDTO
        {
            FirstName = "Updated",
            SecondName = "Second",
            ThirdName = "Third",
            LastName = "Last",
            PhoneNumber = "0591111111",
            CityId = 2
        }, "user-1");

        Assert.Equal("Updated", user.FirstName);
        Assert.Equal("0591111111", user.PhoneNumber);
        Assert.Equal(2, user.CityId);
        Assert.Equal(RolesSelector.Citizen, result.Role);
        userManager.Verify(x => x.UpdateAsync(user), Times.Once);
    }

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

    [Fact]
    public async Task UserService_ChangePasswordAsync_WhenValid_DelegatesToUserManager()
    {
        var user = CreateUser();
        var userManager = IdentityMockFactory.CreateUserManager();
        userManager.Setup(x => x.FindByIdAsync("user-1")).ReturnsAsync(user);
        userManager.Setup(x => x.ChangePasswordAsync(user, "P@ssw0rd!", "N3wP@ssw0rd!"))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
        var service = ServiceTestFactory.CreateUserService(userManager, TestDbContextFactory.Create());

        await service.ChangePasswordAsync(new ChangePasswordDTO
        {
            CurrentPassword = "P@ssw0rd!",
            NewPassword = "N3wP@ssw0rd!"
        }, "user-1");

        userManager.Verify(x => x.ChangePasswordAsync(user, "P@ssw0rd!", "N3wP@ssw0rd!"), Times.Once);
    }

    [Fact]
    public async Task UserService_ChangeAccountStatus_WhenStatusInvalid_ThrowsValidationException()
    {
        var service = ServiceTestFactory.CreateUserService(IdentityMockFactory.CreateUserManager(), TestDbContextFactory.Create());

        await Assert.ThrowsAsync<ValidationException>(() => service.ChangeAccountStatus(
            new ChangeAccountStatusDTO { Status = (AccountStatus)999 }, "user-1"));
    }

    [Fact]
    public async Task UserService_ChangeAccountStatus_WhenUserExists_UpdatesStatusAndSaves()
    {
        var user = CreateUser();
        var userManager = IdentityMockFactory.CreateUserManager();
        userManager.Setup(x => x.FindByIdAsync("user-1")).ReturnsAsync(user);
        userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
        var service = ServiceTestFactory.CreateUserService(userManager, TestDbContextFactory.Create());

        await service.ChangeAccountStatus(new ChangeAccountStatusDTO { Status = AccountStatus.Inactive }, "user-1");

        Assert.Equal(AccountStatus.Inactive, user.AccountStatus);
        userManager.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public void UserService_GetAccountStatusValues_ReturnsAllAccountStatuses()
    {
        var service = ServiceTestFactory.CreateUserService(IdentityMockFactory.CreateUserManager(), TestDbContextFactory.Create());

        var result = service.GetAccountStatusValues();

        Assert.Equal(Enum.GetValues<AccountStatus>().Length, result.Count);
        Assert.Contains(result, x => x.StatuName == AccountStatus.Active.ToString());
    }

    private static AppUser CreateUser()
    {
        return new AppUser
        {
            Id = "user-1",
            FirstName = "First",
            SecondName = "Second",
            ThirdName = "Third",
            LastName = "Last",
            UserName = "first",
            Email = "first@test.com",
            PhoneNumber = "0590000000",
            Birthdate = DateTime.UtcNow.AddYears(-25),
            CityId = 1,
            AccountStatus = AccountStatus.Active
        };
    }
}
