using BL.DTO.General;
using BL.DTO.News;
using BL.Helper;
using BL.Services;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;
using Moq;
using TestProject.Support;

namespace TestProject.Services;

public class NewsItemServiceTests
{
    [Fact]
    public async Task NewsItemService_AddAsync_CreatesDraftByDefault()
    {
        NewsItem? capturedNews = null;
        var newsRepo = new Mock<INewsItemRepository>();
        newsRepo.Setup(x => x.AddAsync(It.IsAny<NewsItem>()))
            .Callback<NewsItem>(news =>
            {
                news.Id = 7;
                capturedNews = news;
            })
            .Returns(Task.CompletedTask);
        newsRepo.Setup(x => x.GetByIdAsync(7)).ReturnsAsync(() => CreateNewsItem(capturedNews));
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(CreateIncident());
        var service = CreateService(newsRepo, incidentRepo);

        var result = await service.AddAsync(CreateAddDto(), CreateCurrentUser());

        Assert.Equal(7, result.Id);
        Assert.False(capturedNews?.IsPublished);
        Assert.Null(capturedNews?.PublishDate);
        Assert.Equal("manager-1", capturedNews?.WrittenById);
    }

    [Fact]
    public async Task NewsItemService_AddAsync_WhenIncidentLacksPublicationConsent_ThrowsConflict()
    {
        var newsRepo = new Mock<INewsItemRepository>();
        newsRepo.Setup(x => x.GetByIdAsync(7)).ReturnsAsync(CreateNewsItem());
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(CreateIncident(documentationConsent: true, publicationConsent: false));
        var service = CreateService(newsRepo, incidentRepo);

        await Assert.ThrowsAsync<ConflictException>(() => service.AddAsync(CreateAddDto(), CreateCurrentUser()));
    }

    [Fact]
    public async Task NewsItemService_PublishAsync_WhenValid_SetsPublishedAndPublishDate()
    {
        var news = CreateNewsItem();
        var newsRepo = new Mock<INewsItemRepository>();
        newsRepo.Setup(x => x.GetByIdAsync(7)).ReturnsAsync(news);
        var incidentRepo = new Mock<IIncidentRepository>();
        incidentRepo.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(CreateIncident(documentationConsent: true, publicationConsent: true));
        var service = CreateService(newsRepo, incidentRepo);

        var result = await service.PublishAsync(7, CreateCurrentUser());

        Assert.True(news.IsPublished);
        Assert.NotNull(news.PublishDate);
        Assert.True(result.IsPublished);
        newsRepo.Verify(x => x.Update(news), Times.Once);
    }

    [Fact]
    public async Task NewsItemService_GetWebsiteAbbreviatedNewsAsync_ReturnsCardData()
    {
        var newsRepo = new Mock<INewsItemRepository>();
        newsRepo.Setup(x => x.GetByPageAsync(0, 20, null, null, 1, true))
            .ReturnsAsync((new List<NewsItem> { CreateNewsItem(isPublished: true) }, 1));
        var service = CreateService(newsRepo, new Mock<IIncidentRepository>());

        var result = await service.GetWebsiteAbbreviatedNewsAsync(new PaginationDTO(), cityId: 1);

        Assert.Single(result.Data);
        Assert.Equal("Title", result.Data[0].Title);
        Assert.Equal(1, result.Data[0].CityId);
        Assert.Equal("https://test/news.jpg", result.Data[0].ImageUrl);
    }

    [Fact]
    public async Task NewsItemService_GetWebsiteByIdAsync_WhenDraft_ThrowsDataNotFound()
    {
        var newsRepo = new Mock<INewsItemRepository>();
        newsRepo.Setup(x => x.GetByIdAsync(7)).ReturnsAsync(CreateNewsItem(isPublished: false));
        var service = CreateService(newsRepo, new Mock<IIncidentRepository>());

        await Assert.ThrowsAsync<DataNotFoundException>(() => service.GetWebsiteByIdAsync(7));
    }

    private static NewsItemService CreateService(Mock<INewsItemRepository> newsRepo,
        Mock<IIncidentRepository> incidentRepo,
        Mock<IActivityRepositoy>? activityRepo = null)
    {
        return new NewsItemService(
            newsRepo.Object,
            incidentRepo.Object,
            activityRepo?.Object ?? Mock.Of<IActivityRepositoy>(),
            TestMapper.Create());
    }

    private static AddNewsItemDTO CreateAddDto()
    {
        return new AddNewsItemDTO
        {
            Title = "Title",
            Summary = "Summary",
            Body = "Body",
            ImageUrl = "https://test/news.jpg",
            IncidentId = 2
        };
    }

    private static CurrentUser CreateCurrentUser()
    {
        return new CurrentUser
        {
            UserId = "manager-1",
            Role = RolesSelector.Manager,
            CityId = "1"
        };
    }

    private static Incident CreateIncident(bool documentationConsent = true, bool publicationConsent = true)
    {
        return new Incident
        {
            Id = 2,
            FieldResearcherId = "researcher-1",
            DetailedDescription = "details",
            AreaName = "area",
            AreaClass = AreaClass.A,
            AreaType = AreaType.Residential,
            DateOfOccurrence = DateTime.UtcNow.AddDays(-3),
            CityId = 1,
            LocationLat = 31.91,
            LocationLng = 35.20,
            DocumentationConsent = documentationConsent,
            PublicationConsent = publicationConsent,
            SensitivityScore = 5
        };
    }

    private static NewsItem CreateNewsItem(NewsItem? source = null, bool isPublished = false)
    {
        var news = source ?? new NewsItem
        {
            Id = 7,
            Title = "Title",
            Summary = "Summary",
            Body = "Body",
            ImageUrl = "https://test/news.jpg",
            IncidentId = 2,
            IsPublished = isPublished,
            PublishDate = isPublished ? DateTime.UtcNow : null,
            WritingDate = DateTime.UtcNow.AddDays(-1),
            WrittenById = "manager-1"
        };

        news.Incident = CreateIncident();
        news.WrittenBy = new AppUser
        {
            Id = "manager-1",
            FirstName = "Manager",
            SecondName = "Second",
            ThirdName = "Third",
            LastName = "One",
            UserName = "manager",
            Email = "manager@test.com",
            PhoneNumber = "0590000000",
            Birthdate = DateTime.UtcNow.AddYears(-30),
            CityId = 1
        };

        return news;
    }
}
