using BL.DTO.Activity;
using BL.DTO.City;
using BL.DTO.Evidence;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.DTO.InitialIncidentReport;
using BL.DTO.LegalNote;
using BL.DTO.Question;
using BL.DTO.User;
using BL.Services.Interfaces;
using DAL.Enums;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;
using TestProject.Support;

namespace TestProject.Controllers;

public class ActivityControllerTests
{
    [Fact]
    public async Task ActivityController_GetByPage_ReturnsPagedActivities()
    {
        var service = new Mock<IActivityService>();
        service.Setup(x => x.GetByPageAsync(It.IsAny<PaginationDTO>(), "created", ActivityType.Add))
            .ReturnsAsync(new PagedResultDTO<List<ReturnActivityDTO>>
            {
                Data = [new ReturnActivityDTO { Id = 1, Description = "created", MadeById = "u1" }],
                Page = 2,
                PageSize = 10,
                TotalCount = 1
            });

        var result = await new ActivityController(service.Object)
            .GetByPageAsync(new PaginationDTO { Page = 2, PageSize = 10 }, "created", ActivityType.Add);

        Assert.IsType<OkObjectResult>(result);
        service.Verify(x => x.GetByPageAsync(It.Is<PaginationDTO>(p => p.Page == 2 && p.PageSize == 10), "created", ActivityType.Add), Times.Once);
    }
}
