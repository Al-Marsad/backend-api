using BL.DTO.User;
using BL.Helper;
using BL.Services;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using TestProject.Support;

namespace TestProject.Services;

public class AuthenticationServiceTests
{
    [Fact]
    public async Task AuthenticationService_Regsiter_CreatesUserAssignsRoleAndLogsActivity()
    {
        var userManager = IdentityMockFactory.CreateUserManager();
        var signInManager = IdentityMockFactory.CreateSignInManager(userManager.Object);
        var activityRepo = new Mock<IActivityRepositoy>();
        AppUser? createdUser = null;

        userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), "P@ssw0rd!"))
            .Callback<AppUser, string>((user, _) => createdUser = user)
            .ReturnsAsync(IdentityResult.Success);
        userManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), RolesSelector.Admin))
            .ReturnsAsync(IdentityResult.Success);

        var service = CreateService(userManager, signInManager, activityRepo: activityRepo);

        var result = await service.Regsiter(CreateUserDto(), RolesSelector.Admin);

        Assert.Equal(RolesSelector.Admin, result.Role);
        Assert.Equal("first@test.com", createdUser?.Email);
        userManager.Verify(x => x.AddToRoleAsync(It.IsAny<AppUser>(), RolesSelector.Admin), Times.Once);
        activityRepo.Verify(x => x.AddAsync(It.Is<Activity>(a => a.Type == ActivityType.Add)), Times.Once);
    }

    [Fact]
    public async Task AuthenticationService_Login_WhenEmailDoesNotExist_ThrowsUnauthorizedException()
    {
        var userManager = IdentityMockFactory.CreateUserManager();
        userManager.Setup(x => x.FindByEmailAsync("missing@test.com")).ReturnsAsync((AppUser?)null);

        var service = ServiceTestFactory.CreateAuthenticationService(userManager);

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            service.Login(new LoginUserDTO { Email = "missing@test.com", Password = "P@ssw0rd!" }));
    }

    [Fact]
    public async Task AuthenticationService_Login_WithValidCredentials_GeneratesTokensAndPersistsRefreshToken()
    {
        var user = CreateUser();
        var userManager = IdentityMockFactory.CreateUserManager();
        var signInManager = IdentityMockFactory.CreateSignInManager(userManager.Object);
        var jwtService = new Mock<IJwtService>();

        userManager.Setup(x => x.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync([RolesSelector.Admin]);
        userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        signInManager.Setup(x => x.CheckPasswordSignInAsync(user, "P@ssw0rd!", true))
            .ReturnsAsync(SignInResult.Success);
        jwtService.Setup(x => x.GenerateAccessToken(user, It.IsAny<IList<string>>())).Returns("access-token");
        jwtService.Setup(x => x.GenerateRefreshToken()).Returns("refresh-token");

        var service = CreateService(userManager, signInManager, jwtService);

        var result = await service.Login(new LoginUserDTO { Email = user.Email!, Password = "P@ssw0rd!" });

        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.Equal("refresh-token", user.RefreshToken);
        Assert.Equal(RolesSelector.Admin, result.Role);
        userManager.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task AuthenticationService_Refresh_WithValidRefreshToken_RotatesTokensAndPersistsChanges()
    {
        var user = CreateUser();
        user.RefreshToken = "old-refresh";
        user.RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1);
        var userManager = IdentityMockFactory.CreateUserManager(new[] { user }.AsQueryable());
        var signInManager = IdentityMockFactory.CreateSignInManager(userManager.Object);
        var jwtService = new Mock<IJwtService>();

        userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync([RolesSelector.Admin]);
        userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        jwtService.Setup(x => x.GenerateAccessToken(user, It.IsAny<IList<string>>())).Returns("new-access");
        jwtService.Setup(x => x.GenerateRefreshToken()).Returns("new-refresh");

        var service = CreateService(userManager, signInManager, jwtService);

        var result = await service.Refresh("old-refresh");

        Assert.Equal("new-access", result.AccessToken);
        Assert.Equal("new-refresh", result.RefreshToken);
        Assert.Equal("new-refresh", user.RefreshToken);
        userManager.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task AuthenticationService_Logout_WithValidRefreshToken_ClearsStoredRefreshToken()
    {
        var user = CreateUser();
        user.RefreshToken = "refresh-token";
        user.RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1);
        var userManager = IdentityMockFactory.CreateUserManager(new[] { user }.AsQueryable());
        var signInManager = IdentityMockFactory.CreateSignInManager(userManager.Object);

        userManager.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        var service = CreateService(userManager, signInManager);

        await service.Logout("refresh-token");

        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpirationTime);
        userManager.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    private static AuthenticationService CreateService(
        Mock<UserManager<AppUser>> userManager,
        Mock<SignInManager<AppUser>> signInManager,
        Mock<IJwtService>? jwtService = null,
        Mock<IActivityRepositoy>? activityRepo = null)
    {
        return new AuthenticationService(
            signInManager.Object,
            userManager.Object,
            jwtService?.Object ?? Mock.Of<IJwtService>(),
            CreateConfiguration(),
            activityRepo?.Object ?? Mock.Of<IActivityRepositoy>(),
            TestMapper.Create());
    }

    private static AddUserDTO CreateUserDto()
    {
        return new AddUserDTO
        {
            FirstName = "First",
            SecondName = "Second",
            ThirdName = "Third",
            LastName = "Last",
            PhoneNumber = "0590000000",
            UserName = "first",
            Birthdate = DateTime.UtcNow.AddYears(-25),
            CityId = 1,
            Email = "first@test.com",
            Password = "P@ssw0rd!"
        };
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

    private static IConfiguration CreateConfiguration()
    {
        var values = new Dictionary<string, string?>
        {
            ["Jwt:ExpiryInMinutes"] = "5",
            ["Jwt:ExpiryInDays"] = "7"
        };

        return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
    }
}
