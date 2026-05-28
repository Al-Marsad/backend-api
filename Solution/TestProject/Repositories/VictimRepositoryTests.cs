using DAL.Entities;
using DAL.Enums;
using DAL.Repositories;
using TestProject.Support;

namespace TestProject.Repositories;

public class VictimRepositoryTests
{
    [Fact]
    public async Task VictimRepository_GetByNationalIdAsync_ReturnsMatchingVictim()
    {
        await using var db = TestDbContextFactory.Create();
        db.Victims.Add(new Victim
        {
            FirstName = "First",
            SecondName = "Second",
            ThirdName = "Third",
            LastName = "Last",
            NationalId = "789",
            PhoneNumber = "0592222222"
        });
        await db.SaveChangesAsync();
        var repo = new VictimRepository(db);

        var victim = await repo.GetByNationalIdAsync("789");

        Assert.NotNull(victim);
        Assert.Equal("789", victim.NationalId);
    }
}
