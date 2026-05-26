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

public class QuestionServiceTests
{
    [Fact]
    public async Task QuestionService_GetQuestionByIncidentClassTypeAsync_WhenIdsEmpty_ThrowsValidationException()
    {
        var service = new QuestionService(Mock.Of<IQuestionRepository>(), TestMapper.Create());

        await Assert.ThrowsAsync<ValidationException>(() => service.GetQuestionByIncidentClassTypeAsync([]));
    }

    [Fact]
    public async Task QuestionService_GetQuestionByIncidentClassTypeAsync_ReturnsMappedQuestions()
    {
        var repo = new Mock<IQuestionRepository>();
        repo.Setup(x => x.GetFullQuestionByIncidentClassTypeAsync(It.IsAny<int[]>()))
            .ReturnsAsync([new Question { Id = 1, QuestionBody = "Question" }]);
        var service = new QuestionService(repo.Object, TestMapper.Create());

        var result = await service.GetQuestionByIncidentClassTypeAsync([1]);

        Assert.Single(result);
        Assert.Equal("Question", result[0].QuestionBody);
    }
}
