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

public class LegalNoteServiceTests
{
    [Fact]
    public async Task LegalNoteService_AddAsync_SetsLegalTeamMemberAndSaves()
    {
        var repo = new Mock<ILegalNotesRepository>();
        LegalTeamMemberNote? added = null;
        repo.Setup(x => x.AddAsync(It.IsAny<LegalTeamMemberNote>()))
            .Callback<LegalTeamMemberNote>(note => added = note)
            .Returns(Task.CompletedTask);
        var service = new LegalNoteService(repo.Object, TestMapper.Create());

        var result = await service.AddAsync("legal-1", new AddLegalNoteDTO { Content = "note", IncidentId = 5 });

        Assert.Equal("legal-1", added?.LegalTeamMemberId);
        Assert.Equal("note", result.Content);
        repo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task LegalNoteService_UpdateAsync_WhenUserDoesNotOwnNote_ThrowsForbidden()
    {
        var repo = new Mock<ILegalNotesRepository>();
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new LegalTeamMemberNote
        {
            Id = 1,
            LegalTeamMemberId = "legal-2",
            Content = "note"
        });
        var service = new LegalNoteService(repo.Object, TestMapper.Create());

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            service.UpdateAsync("legal-1", 1, new UpdateLegalNoteDTO { Content = "updated" }));
    }

    [Fact]
    public async Task LegalNoteService_UpdateAsync_WhenUserOwnsNote_UpdatesContentAndSaves()
    {
        var note = new LegalTeamMemberNote { Id = 1, LegalTeamMemberId = "legal-1", Content = "old" };
        var repo = new Mock<ILegalNotesRepository>();
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(note);
        var service = new LegalNoteService(repo.Object, TestMapper.Create());

        var result = await service.UpdateAsync("legal-1", 1, new UpdateLegalNoteDTO { Content = "updated" });

        Assert.Equal("updated", note.Content);
        Assert.Equal("updated", result.Content);
        repo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task LegalNoteService_DeleteAsync_WhenUserOwnsNote_DeletesAndSaves()
    {
        var note = new LegalTeamMemberNote { Id = 1, LegalTeamMemberId = "legal-1", Content = "note" };
        var repo = new Mock<ILegalNotesRepository>();
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(note);
        var service = new LegalNoteService(repo.Object, TestMapper.Create());

        await service.DeleteAsync("legal-1", 1);

        repo.Verify(x => x.Delete(note), Times.Once);
        repo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task LegalNoteService_DeleteAsync_WhenNoteMissing_ThrowsDataNotFoundException()
    {
        var repo = new Mock<ILegalNotesRepository>();
        repo.Setup(x => x.GetByIdAsync(404)).ReturnsAsync((LegalTeamMemberNote?)null);
        var service = new LegalNoteService(repo.Object, TestMapper.Create());

        await Assert.ThrowsAsync<DataNotFoundException>(() => service.DeleteAsync("legal-1", 404));
    }

    [Fact]
    public async Task LegalNoteService_GetByIncident_ReturnsMappedNotes()
    {
        var repo = new Mock<ILegalNotesRepository>();
        repo.Setup(x => x.GetByIncidentIdAsync(5)).ReturnsAsync(
        [
            new LegalTeamMemberNote { Id = 1, IncidentId = 5, LegalTeamMemberId = "legal-1", Content = "note" }
        ]);
        var service = new LegalNoteService(repo.Object, TestMapper.Create());

        var result = await service.GetByIncident(5);

        Assert.Single(result);
        Assert.Equal("note", result[0].Content);
    }
}
