using DAL.Entities;
using DAL.Enums;
using DAL.Repositories;
using BL.Helper;
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

        var (items, total) = await repo.GetIncidentsByPageAndUserIdAsync(
            0, 10, "r1", RolesSelector.FieldResearcher, null, "123", false, true, null, null, null);

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

        var (items, total) = await repo.GetAllIncidentsByPageAsync(0, 10, 3, null, false, null, null, null, 8);

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

    [Fact]
    public async Task IncidentRepository_GetStatsAsync_ReturnsManagerBuckets()
    {
        await using var db = TestDbContextFactory.Create();
        db.Incidents.AddRange(
            new Incident { FieldResearcherId = "r1", DetailedDescription = "pending publication", AreaName = "area", DocumentationConsent = true, PreventModification = true },
            new Incident { FieldResearcherId = "r2", DetailedDescription = "published", AreaName = "area", DocumentationConsent = true, PublicationConsent = true, PreventModification = true },
            new Incident { FieldResearcherId = "r3", DetailedDescription = "locked", AreaName = "area", PreventModification = true },
            new Incident { FieldResearcherId = "r4", DetailedDescription = "open", AreaName = "area" });
        await db.SaveChangesAsync();
        var repo = new IncidentRepository(db);

        var stats = await repo.GetStatsAsync();

        Assert.Equal(1, stats.PendingPublicationCount);
        Assert.Equal(1, stats.PublishedCount);
        Assert.Equal(2, stats.LockedUnpublishedCount);
        Assert.Equal(4, stats.TotalCount);
    }

    [Fact]
    public async Task IncidentRepository_GetMyStatsAsync_ReturnsLegalTeamBuckets()
    {
        await using var db = TestDbContextFactory.Create();
        db.Incidents.AddRange(
            new Incident { FieldResearcherId = "r1", DetailedDescription = "pending", AreaName = "area" },
            new Incident { FieldResearcherId = "r2", LegalTeamMemberId = "legal-1", DetailedDescription = "under review", AreaName = "area" },
            new Incident { FieldResearcherId = "r3", LegalTeamMemberId = "legal-1", DetailedDescription = "reviewed", AreaName = "area", PreventModification = true },
            new Incident { FieldResearcherId = "r4", LegalTeamMemberId = "legal-1", DetailedDescription = "sent", AreaName = "area", DocumentationConsent = true, PreventModification = true },
            new Incident { FieldResearcherId = "r5", LegalTeamMemberId = "legal-2", DetailedDescription = "other", AreaName = "area" });
        await db.SaveChangesAsync();
        var repo = new IncidentRepository(db);

        var stats = await repo.GetMyStatsAsync("legal-1");

        Assert.Equal(1, stats.PendingReviewCount);
        Assert.Equal(1, stats.UnderReviewCount);
        Assert.Equal(1, stats.ReviewedCount);
        Assert.Equal(1, stats.SentToManagerCount);
    }

    [Fact]
    public async Task IncidentRepository_GetPublicStatsAsync_CountsPublishedAndPendingReview()
    {
        await using var db = TestDbContextFactory.Create();
        var now = DateTime.UtcNow;
        db.Incidents.AddRange(
            new Incident { FieldResearcherId = "r1", DetailedDescription = "published this month", AreaName = "area", CityId = 1, PublicationConsent = true, CreationDate = now },
            new Incident { FieldResearcherId = "r2", DetailedDescription = "published old", AreaName = "area", CityId = 2, PublicationConsent = true, CreationDate = now.AddMonths(-2) },
            new Incident { FieldResearcherId = "r3", DetailedDescription = "pending", AreaName = "area", CityId = 2 });
        await db.SaveChangesAsync();
        var repo = new IncidentRepository(db);

        var stats = await repo.GetPublicStatsAsync();

        Assert.Equal(2, stats.TotalIncidents);
        Assert.Equal(2, stats.RegionsAffected);
        Assert.Equal(1, stats.ReportsThisMonth);
        Assert.Equal(1, stats.PendingReview);
    }

    [Fact]
    public async Task IncidentRepository_GetAnalyticsAsync_GroupsPublishedIncidents()
    {
        await using var db = TestDbContextFactory.Create();
        var city = new City { Id = 9, ArabicName = "City AR", EnglishName = "City EN" };
        db.Cities.Add(city);
        db.Incidents.AddRange(
            new Incident
            {
                FieldResearcherId = "r1",
                DetailedDescription = "published one",
                AreaName = "area",
                City = city,
                PublicationConsent = true,
                CreationDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Incident
            {
                FieldResearcherId = "r2",
                DetailedDescription = "published two",
                AreaName = "area",
                City = city,
                PublicationConsent = true,
                CreationDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new Incident
            {
                FieldResearcherId = "r3",
                DetailedDescription = "draft",
                AreaName = "area",
                City = city,
                CreationDate = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc)
            });
        await db.SaveChangesAsync();
        var repo = new IncidentRepository(db);

        var analytics = await repo.GetAnalyticsAsync();

        Assert.Single(analytics.ByMonth);
        Assert.Equal(2, analytics.ByMonth[0].Count);
        Assert.Single(analytics.ByYear);
        Assert.Equal(2026, analytics.ByYear[0].Year);
        Assert.Single(analytics.ByCity);
        Assert.Equal("City EN", analytics.ByCity[0].EnglishName);
        Assert.Equal(2, analytics.ByCity[0].Count);
    }
}
