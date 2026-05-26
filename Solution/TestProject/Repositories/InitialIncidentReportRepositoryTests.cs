using DAL.Entities;
using DAL.Enums;
using DAL.Repositories;
using TestProject.Support;

namespace TestProject.Repositories;

public class InitialIncidentReportRepositoryTests
{
    [Fact]
    public async Task InitialIncidentReportRepository_GetPageAsync_FiltersByUserStatusAndCity()
    {
        await using var db = TestDbContextFactory.Create();
        db.Cities.AddRange(
            new City { Id = 1, ArabicName = "مدينة 1", EnglishName = "City 1" },
            new City { Id = 2, ArabicName = "مدينة 2", EnglishName = "City 2" });
        db.InitialIncidentReports.AddRange(
            new InitialIncidentReport { InitialDescription = "mine", CitizenReporterId = "citizen-1", CityId = 1, Status = InitialIncidentReportStatus.UNASSIGNED },
            new InitialIncidentReport { InitialDescription = "assigned", CitizenReporterId = "citizen-2", CityId = 2, Status = InitialIncidentReportStatus.ASSIGNED });
        await db.SaveChangesAsync();
        var repo = new InitialIncidentReportRepository(db);

        var (items, total) = await repo.GetPageAsync(0, 10, "citizen-1", InitialIncidentReportStatus.UNASSIGNED, 1);

        Assert.Equal(1, total);
        Assert.Single(items);
        Assert.Equal("mine", items[0].InitialDescription);
    }

    [Fact]
    public async Task InitialIncidentReportRepository_GetAssignedReportsAsync_FiltersAssignedReportsByResearcherAndSearch()
    {
        await using var db = TestDbContextFactory.Create();
        db.InitialIncidentReports.AddRange(
            new InitialIncidentReport { InitialDescription = "olive field", CitizenReporterId = "c1", FieldResearcherId = "r1", Status = InitialIncidentReportStatus.ASSIGNED },
            new InitialIncidentReport { InitialDescription = "school", CitizenReporterId = "c2", FieldResearcherId = "r1", Status = InitialIncidentReportStatus.UNASSIGNED },
            new InitialIncidentReport { InitialDescription = "olive road", CitizenReporterId = "c3", FieldResearcherId = "r2", Status = InitialIncidentReportStatus.ASSIGNED });
        await db.SaveChangesAsync();
        var repo = new InitialIncidentReportRepository(db);

        var (items, total) = await repo.GetAssignedReportsAsync("r1", 0, 10, "olive");

        Assert.Equal(1, total);
        Assert.Single(items);
        Assert.Equal("olive field", items[0].InitialDescription);
    }

    [Fact]
    public async Task InitialIncidentReportRepository_HasIncident_ReturnsTrueWhenIncidentExists()
    {
        await using var db = TestDbContextFactory.Create();
        db.Incidents.Add(new Incident
        {
            InitialIncidentReportId = 5,
            FieldResearcherId = "r1",
            DetailedDescription = "details",
            AreaName = "area"
        });
        await db.SaveChangesAsync();
        var repo = new InitialIncidentReportRepository(db);

        Assert.True(await repo.HasIncident(5));
    }
}
