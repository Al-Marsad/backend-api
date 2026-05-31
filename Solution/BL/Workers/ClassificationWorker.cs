using BL.Models;
using BL.Queue.Interfaces;
using BL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DAL.DBContext;

namespace BL.Workers;

public class ClassificationWorker : BackgroundService
{
    private readonly IIncidentClassificationQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ClassificationWorker> _logger;

    public ClassificationWorker(
        IIncidentClassificationQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<ClassificationWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Classification worker started.");

        await foreach (var incidentId in _queue.ReadAllAsync(stoppingToken))
        {
            await ProcessWithRetryAsync(incidentId, stoppingToken);
        }

        _logger.LogInformation("Classification worker stopped.");
    }

    private async Task ProcessWithRetryAsync(string incidentId, CancellationToken ct)
    {
        const int maxRetries = 3;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                // Each incident gets its own DI scope (fresh DbContext, services, etc.)
                await using var scope = _scopeFactory.CreateAsyncScope();

                var db = scope.ServiceProvider.GetRequiredService<AlMarsadDbContext>();
                var embeddings = scope.ServiceProvider.GetRequiredService<IEmbeddingService>();
                var vectorStore = scope.ServiceProvider.GetRequiredService<IVectorStoreService<RomeStatuteArticle, MatchedArticle>>();
                var classifier = scope.ServiceProvider.GetRequiredService<
                    IClassificationService<IncidentClassificationInput, MatchedArticle, ClassificationResult>>();

                var incident = await db.Incidents
                    .Include(i => i.PersonalVictimTestimonies)
                    .FirstOrDefaultAsync(i => i.Id.ToString() == incidentId, ct);

                if (incident is null)
                {
                    _logger.LogWarning("Incident {Id} not found — skipping.", incidentId);
                    return;
                }


                var classificationInput = new IncidentClassificationInput
                {
                    DetailedDescription = incident.DetailedDescription,
                    PerpetratorDescription = incident.PerpetratorDescription,
                    QuestionnaireJSON = incident.QuestionnaireJSON,
                    PersonalNarratives = incident.PersonalVictimTestimonies
                                                .Select(t => t.PersonalNarrative)
                                                .Where(n => n is not null)
                                                .ToList()!
                };

               
                var embedding = await embeddings.EmbedAsync(
                    classificationInput.BuildFullContext());

                var matchedArticles = await vectorStore.SearchSimilarAsync(embedding, topK: 8);

                var result = await classifier.ClassifyAsync(classificationInput, matchedArticles);

                incident.AIClassification = result.Analysis;
                await db.SaveChangesAsync(ct);

                _logger.LogInformation(
                    "Incident {Id} classified successfully after {Attempt} attempt(s).",
                    incidentId, attempt);

                return;
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                _logger.LogWarning(ex,
                    $"Attempt {attempt}/{maxRetries} failed for incident {incidentId}. Retrying in {delay.TotalSeconds}s...",
                    attempt, maxRetries, incidentId, delay.TotalSeconds);

                await Task.Delay(delay, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"All {maxRetries} retries exhausted for incident {incidentId}. Marking as Failed.",
                    maxRetries, incidentId);

                await MarkAsFailedAsync(incidentId, ct);
            }
        }
    }

    private async Task MarkAsFailedAsync(string incidentId, CancellationToken ct)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AlMarsadDbContext>();
            var incident = await db.Incidents.FirstOrDefaultAsync(i => i.Id.ToString() == incidentId, ct);

            if (incident is not null)
            {
                incident.AIClassification = "فشل التحليل القانوني";
                await db.SaveChangesAsync(ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Could not mark incident {Id} as Failed.", incidentId);
        }
    }
}