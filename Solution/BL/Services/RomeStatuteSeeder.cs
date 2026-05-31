using BL.Models;
using BL.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BL.Services;

public class RomeStatuteSeeder : ISeeder<RomeStatuteArticle>
{
    private readonly IVectorStoreService<RomeStatuteArticle, MatchedArticle> _vectorStore;
    private readonly IEmbeddingService _embeddings;
    private readonly ILogger<RomeStatuteSeeder> _logger;

    public RomeStatuteSeeder(
        IVectorStoreService<RomeStatuteArticle, MatchedArticle> vectorStore,
        IEmbeddingService embeddings,
        ILogger<RomeStatuteSeeder> logger)
    {
        _vectorStore = vectorStore;
        _embeddings = embeddings;
        _logger = logger;
    }

    public async Task<SeedResult> SeedAsync(List<RomeStatuteArticle> articles)
    {
        await _vectorStore.EnsureCollectionAsync();

        _logger.LogInformation("Starting to seed {Count} Rome Statute articles...", articles.Count);

        int success = 0, failed = 0;
        int count = 1;

        foreach (var article in articles)
        {
            try
            {
                var embedding = await _embeddings.EmbedAsync(article.TextToVectorize);
                await _vectorStore.UpsertArticleAsync(article, embedding);

                _logger.LogInformation("[{count}/{Total}] ✓ {Title}",
                    count++, articles.Count, article.Title);

                success++;

                // Respect Gemini free tier rate limits (~1500 req/day, ~1/sec burst)
                await Task.Delay(400);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{count}] ✗ Failed to seed: {Title}",
                    count, article.Title);
                failed++;
            }
        }

        _logger.LogInformation(
            "Seeding complete. Success: {Success}, Failed: {Failed}",
            success, failed);

        return new SeedResult { Success = success, Failed = failed };
    }
}

public class SeedResult
{
    public int Success { get; set; }
    public int Failed { get; set; }
}