using BL.Helper;
using BL.Services;
using BL.Services.Interfaces;
using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace TestProject.Support;

internal static class ServiceTestFactory
{
    public static AuthenticationService CreateAuthenticationService(Mock<Microsoft.AspNetCore.Identity.UserManager<AppUser>> userManager)
    {
        var signInManager = IdentityMockFactory.CreateSignInManager(userManager.Object);
        return new AuthenticationService(
            signInManager.Object,
            userManager.Object,
            Mock.Of<IJwtService>(),
            CreateJwtConfiguration(),
            Mock.Of<IActivityRepositoy>(),
            TestMapper.Create());
    }

    public static IncidentService CreateIncidentService(
        Mock<IIncidentRepository> incidentRepo,
        Mock<IVictimRepository>? victimRepo = null,
        Mock<IInitialIncidentReportRepository>? initialReportRepo = null,
        Mock<ICloudinaryService>? cloudinaryService = null,
        Mock<IActivityRepositoy>? activityRepo = null)
    {
        return new IncidentService(
            incidentRepo.Object,
            victimRepo?.Object ?? Mock.Of<IVictimRepository>(),
            TestMapper.Create(),
            initialReportRepo?.Object ?? Mock.Of<IInitialIncidentReportRepository>(),
            cloudinaryService?.Object ?? Mock.Of<ICloudinaryService>(),
            activityRepo?.Object ?? Mock.Of<IActivityRepositoy>());
    }

    public static UserService CreateUserService(Mock<Microsoft.AspNetCore.Identity.UserManager<AppUser>> userManager, AlMarsadDbContext dbContext)
    {
        var mapper = TestMapper.Create();
        return new UserService(
            userManager.Object,
            mapper,
            dbContext,
            new DTOBuilder(mapper, userManager.Object),
            Mock.Of<IActivityRepositoy>());
    }

    public static IConfiguration CreateJwtConfiguration()
    {
        var values = new Dictionary<string, string?>
        {
            ["Jwt:SecretKey"] = "0123456789ABCDEF0123456789ABCDEF",
            ["Jwt:Issuer"] = "test-issuer",
            ["Jwt:ExpiryInMinutes"] = "5",
            ["Jwt:ExpiryInDays"] = "7",
            ["Jwt:Audiences:0"] = "test-audience"
        };

        return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
    }

}
