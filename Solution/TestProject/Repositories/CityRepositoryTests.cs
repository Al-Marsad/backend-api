using DAL.Entities;
using DAL.Enums;
using DAL.Repositories;
using TestProject.Support;

namespace TestProject.Repositories;

public class CityRepositoryTests
{
    [Fact]
    public async Task CityRepository_AddGetUpdateDeleteAndCount_Works()
    {
        await using var db = TestDbContextFactory.Create();
        var repo = new CityRepository(db);
        var city = new City { ArabicName = "رام الله", EnglishName = "Ramallah" };

        await repo.AddAsync(city);
        await repo.SaveAsync();

        Assert.Equal(1, await repo.CountAsync());
        Assert.Single(await repo.GetAllAsync("Ram"));

        city.EnglishName = "Al-Bireh";
        repo.Update(city);
        await repo.SaveAsync();
        Assert.Equal("Al-Bireh", (await repo.GetByIdAsync(city.Id))?.EnglishName);

        repo.Delete(city);
        await repo.SaveAsync();
        Assert.Equal(0, await repo.CountAsync());
    }
}
