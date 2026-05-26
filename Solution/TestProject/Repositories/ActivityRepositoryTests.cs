using DAL.Entities;
using DAL.Enums;
using DAL.Repositories;
using TestProject.Support;

namespace TestProject.Repositories;

public class ActivityRepositoryTests
{
    [Fact]
    public async Task ActivityRepository_GetByPageAsync_FiltersBySearchAndType()
    {
        await using var db = TestDbContextFactory.Create();
        db.Activities.AddRange(
            new Activity { Description = "created report", MadeById = "u1", Type = ActivityType.Add, CreationDate = DateTime.UtcNow.AddMinutes(-2) },
            new Activity { Description = "updated report", MadeById = "u2", Type = ActivityType.Update, CreationDate = DateTime.UtcNow.AddMinutes(-1) });
        await db.SaveChangesAsync();
        var repo = new ActivityRepository(db);

        var (items, total) = await repo.GetByPageAsync(0, 10, "updated", ActivityType.Update);

        Assert.Equal(1, total);
        Assert.Single(items);
        Assert.Equal(ActivityType.Update, items[0].Type);
    }

    [Fact]
    public async Task ActivityRepository_GetByPageAsync_WhenSkipOrTakeNegative_ReturnsEmptyResult()
    {
        await using var db = TestDbContextFactory.Create();
        var repo = new ActivityRepository(db);

        var (items, total) = await repo.GetByPageAsync(-1, 10);

        Assert.Empty(items);
        Assert.Equal(0, total);
    }
}
