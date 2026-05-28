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

public class JwtServiceTests
{
    [Fact]
    public void JwtService_GenerateAccessToken_IncludesUserAndRoleClaims()
    {
        var service = new JwtService(ServiceTestFactory.CreateJwtConfiguration());
        var user = new AppUser { Id = "user-1", UserName = "tester", CityId = 3 };

        var token = service.GenerateAccessToken(user, [RolesSelector.Admin]);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Contains(jwt.Claims, c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == RolesSelector.Admin);
        Assert.Contains(jwt.Claims, c => c.Type == "CityId" && c.Value == "3");
    }
}
