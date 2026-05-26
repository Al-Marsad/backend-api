using DAL.Entities;
using DAL.Enums;
using DAL.Repositories;
using TestProject.Support;

namespace TestProject.Repositories;

public class IncidentRepositoryTests
{
    [Fact]
    public async Task IncidentRepository_GetFieldResearcherIncidentsByPageAsync_FiltersByVictimAndConsent()
    {
        await using var db = TestDbContextFactory.Create();
        var victim = new Victim
        {
            FirstName = "First",
            SecondName = "Second",
            ThirdName = "Third",
            LastName = "Last",
            NationalId = "123",
            PhoneNumber = "0590000000"
        };
        db.Incidents.AddRange(
            new Incident
            {
                FieldResearcherId = "r1",
                DetailedDescription = "matching",
                AreaName = "area",
                DocumentationConsent = true,
                PersonalVictimTestimonies =
                [
                    new PersonalVictimTestimonie { Victim = victim, IssueDate = DateTime.UtcNow, InjuryStatus = InjuryStatus.None }
                ]
            },
            new Incident { FieldResearcherId = "r1", DetailedDescription = "other", AreaName = "area", DocumentationConsent = false },
            new Incident { FieldResearcherId = "r2", DetailedDescription = "not mine", AreaName = "area", DocumentationConsent = true });
        await db.SaveChangesAsync();
        var repo = new IncidentRepository(db);

        var (items, total) = await repo.GetFieldResearcherIncidentsByPageAsync(0, 10, "r1", "123", false, true, null);

        Assert.Equal(1, total);
        Assert.Single(items);
        Assert.Equal("matching", items[0].DetailedDescription);
    }

    [Fact]
    public async Task IncidentRepository_GetAllIncidentsByPageAsync_FiltersByCityAndSensitivity()
    {
        await using var db = TestDbContextFactory.Create();
        db.Incidents.AddRange(
            new Incident { FieldResearcherId = "r1", DetailedDescription = "one", AreaName = "area", CityId = 3, SensitivityScore = 8 },
            new Incident { FieldResearcherId = "r2", DetailedDescription = "two", AreaName = "area", CityId = 4, SensitivityScore = 8 },
            new Incident { FieldResearcherId = "r3", DetailedDescription = "three", AreaName = "area", CityId = 3, SensitivityScore = 4 });
        await db.SaveChangesAsync();
        var repo = new IncidentRepository(db);

        var (items, total) = await repo.GetAllIncidentsByPageAsync(0, 10, 3, false, null, null, 8);

        Assert.Equal(1, total);
        Assert.Single(items);
        Assert.Equal("one", items[0].DetailedDescription);
    }

    [Fact]
    public async Task IncidentRepository_AddRangeAndGetEvidencesByIncidentId_Works()
    {
        await using var db = TestDbContextFactory.Create();
        var repo = new IncidentRepository(db);

        await repo.AddRangeOfEvidencesAsync(
        [
            new Evidence { IncidentId = 1, Type = EvidenceType.Image, CloudinaryUrl = "https://test/1", CloudinaryPublicId = "one" },
            new Evidence { IncidentId = 2, Type = EvidenceType.Document, CloudinaryUrl = "https://test/2", CloudinaryPublicId = "two" }
        ]);
        await repo.SaveAsync();

        var evidences = await repo.GetEvidencesByIncidentIdAsync(1);

        Assert.Single(evidences);
        Assert.Equal("one", evidences[0].CloudinaryPublicId);
    }

    [Fact]
    public async Task IncidentRepository_GetTestimoniesAndTheirVictimsByIncidentId_IncludesVictim()
    {
        await using var db = TestDbContextFactory.Create();
        db.PersonalVictimTestimonies.Add(new PersonalVictimTestimonie
        {
            IncidentId = 7,
            IssueDate = DateTime.UtcNow,
            InjuryStatus = InjuryStatus.Injured,
            Victim = new Victim
            {
                FirstName = "First",
                SecondName = "Second",
                ThirdName = "Third",
                LastName = "Last",
                NationalId = "456",
                PhoneNumber = "0591111111"
            }
        });
        await db.SaveChangesAsync();
        var repo = new IncidentRepository(db);

        var testimonies = await repo.GetTestimoniesAndTheirVictimsByIncidentIdAsync(7);

        Assert.Single(testimonies);
        Assert.Equal("456", testimonies[0].Victim.NationalId);
    }
}
