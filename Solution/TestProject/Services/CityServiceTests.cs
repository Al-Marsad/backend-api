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

public class CityServiceTests
{
    [Fact]
    public async Task CityService_AddAsync_AddsCityAndSaves()
    {
        var repo = new Mock<ICityRepository>();
        City? addedCity = null;
        repo.Setup(x => x.AddAsync(It.IsAny<City>()))
            .Callback<City>(city => addedCity = city)
            .Returns(Task.CompletedTask);

        var service = new CityService(repo.Object, TestMapper.Create());

        var result = await service.AddAsync(new AddCityDTO { ArabicName = "Ramallah", EnglishName = "Ramallah" });

        Assert.Equal("Ramallah", addedCity?.EnglishName);
        Assert.Equal("Ramallah", result.EnglishName);
        repo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task CityService_DeleteAsync_WhenCityMissing_ThrowsDataNotFoundException()
    {
        var repo = new Mock<ICityRepository>();
        repo.Setup(x => x.GetByIdAsync(404)).ReturnsAsync((City?)null);
        var service = new CityService(repo.Object, TestMapper.Create());

        await Assert.ThrowsAsync<DataNotFoundException>(() => service.DeleteAsync(404));
        repo.Verify(x => x.SaveAsync(), Times.Never);
    }
}
