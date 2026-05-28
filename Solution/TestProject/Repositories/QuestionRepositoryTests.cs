using DAL.Entities;
using DAL.Enums;
using DAL.Repositories;
using TestProject.Support;

namespace TestProject.Repositories;

public class QuestionRepositoryTests
{
    [Fact]
    public async Task QuestionRepository_GetFullQuestionByIncidentClassTypeAsync_FiltersQuestionsByClassType()
    {
        await using var db = TestDbContextFactory.Create();
        var classType = new IncidentClassType { Title = "type" };
        db.Questions.AddRange(
            new Question { QuestionBody = "matching", IncidentClassTypes = [classType] },
            new Question { QuestionBody = "other", IncidentClassTypes = [new IncidentClassType { Title = "other" }] });
        await db.SaveChangesAsync();
        var repo = new QuestionRepository(db);

        var questions = await repo.GetFullQuestionByIncidentClassTypeAsync([classType.Id]);

        Assert.Single(questions);
        Assert.Equal("matching", questions[0].QuestionBody);
        Assert.Single(questions[0].IncidentClassTypes);
    }
}
