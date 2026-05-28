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

public class QuestionControllerTests
{
    [Fact]
    public async Task QuestionController_GetQuestionsByIncidentClassType_ReturnsOk()
    {
        var service = new Mock<IQuestionService>();
        service.Setup(x => x.GetQuestionByIncidentClassTypeAsync(It.Is<int[]>(ids => ids.SequenceEqual(new[] { 1, 2 }))))
            .ReturnsAsync([new ReturnFullQuestionDTO { Id = 1, QuestionBody = "Question" }]);

        var result = await new QuestionController(service.Object).GetQuestionsByIncidentClassType([1, 2]);

        Assert.IsType<OkObjectResult>(result);
    }
}
