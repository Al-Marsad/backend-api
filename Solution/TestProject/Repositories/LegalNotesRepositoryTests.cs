using DAL.Entities;
using DAL.Enums;
using DAL.Repositories;
using TestProject.Support;

namespace TestProject.Repositories;

public class LegalNotesRepositoryTests
{
    [Fact]
    public async Task LegalNotesRepository_GetByIncidentIdAsync_ReturnsOnlyIncidentNotes()
    {
        await using var db = TestDbContextFactory.Create();
        db.LegalTeamMemberNotes.AddRange(
            new LegalTeamMemberNote { IncidentId = 1, LegalTeamMemberId = "legal-1", Content = "one" },
            new LegalTeamMemberNote { IncidentId = 2, LegalTeamMemberId = "legal-1", Content = "two" });
        await db.SaveChangesAsync();
        var repo = new LegalNotesRepository(db);

        var notes = await repo.GetByIncidentIdAsync(1);

        Assert.Single(notes);
        Assert.Equal("one", notes[0].Content);
    }

    [Fact]
    public async Task LegalNotesRepository_AddGetDeleteAndSave_Works()
    {
        await using var db = TestDbContextFactory.Create();
        var repo = new LegalNotesRepository(db);
        var note = new LegalTeamMemberNote { IncidentId = 1, LegalTeamMemberId = "legal-1", Content = "note" };

        await repo.AddAsync(note);
        await repo.SaveAsync();
        Assert.NotNull(await repo.GetByIdAsync(note.Id));

        repo.Delete(note);
        await repo.SaveAsync();
        Assert.Null(await repo.GetByIdAsync(note.Id));
    }
}
