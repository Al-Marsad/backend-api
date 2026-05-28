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

public class ActivityServiceTests
{
    [Fact]
    public async Task ActivityService_GetByPage_RejectsInvalidActivityType()
    {
        var service = new ActivityService(Mock.Of<IActivityRepositoy>(), TestMapper.Create());

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.GetByPageAsync(new PaginationDTO(), activityType: (ActivityType)999));
    }

    [Fact]
    public async Task ActivityService_GetByPage_ReturnsMappedPagedResult()
    {
        var repo = new Mock<IActivityRepositoy>();
        repo.Setup(x => x.GetByPageAsync(10, 10, "updated", ActivityType.Update))
            .ReturnsAsync((
                [new Activity { Id = 1, Description = "updated", MadeById = "u1", Type = ActivityType.Update }],
                25));

        var service = new ActivityService(repo.Object, TestMapper.Create());

        var result = await service.GetByPageAsync(new PaginationDTO { Page = 2, PageSize = 10 }, "updated", ActivityType.Update);

        Assert.Equal(2, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(25, result.TotalCount);
        Assert.Single(result.Data);
    }
}
