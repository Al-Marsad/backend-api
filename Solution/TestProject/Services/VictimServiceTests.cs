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

public class VictimServiceTests
{
    [Fact]
    public async Task VictimService_VictimExists_WhenNationalIdBlank_ThrowsValidationException()
    {
        var service = new VictimService(Mock.Of<IVictimRepository>());

        await Assert.ThrowsAsync<ValidationException>(() => service.VictimExists(" "));
    }

    [Fact]
    public async Task VictimService_VictimExists_ReturnsTrueWhenVictimFound()
    {
        var repo = new Mock<IVictimRepository>();
        repo.Setup(x => x.GetByNationalIdAsync("123")).ReturnsAsync(new Victim { Id = 1, NationalId = "123" });
        var service = new VictimService(repo.Object);

        Assert.True(await service.VictimExists("123"));
    }
}
