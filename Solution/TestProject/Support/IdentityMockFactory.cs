using DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace TestProject.Support;

internal static class IdentityMockFactory
{
    public static Mock<UserManager<AppUser>> CreateUserManager(IQueryable<AppUser>? users = null)
    {
        var store = new Mock<IUserStore<AppUser>>();
        var manager = new Mock<UserManager<AppUser>>(
            store.Object,
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<AppUser>>(),
            Array.Empty<IUserValidator<AppUser>>(),
            Array.Empty<IPasswordValidator<AppUser>>(),
            Mock.Of<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<AppUser>>>());

        manager.Setup(x => x.Users).Returns(users ?? new List<AppUser>().AsQueryable());
        return manager;
    }

    public static Mock<SignInManager<AppUser>> CreateSignInManager(UserManager<AppUser> userManager)
    {
        return new Mock<SignInManager<AppUser>>(
            userManager,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<ILogger<SignInManager<AppUser>>>(),
            Mock.Of<IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<AppUser>>());
    }
}
