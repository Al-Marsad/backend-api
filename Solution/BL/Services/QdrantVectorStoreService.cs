using BL.Models;
using BL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace BL.Services
{
    public class QdrantVectorStoreService : IVectorStoreService<RomeStatuteArticle, MatchedArticle>
    {
        private readonly QdrantClient _client;
        private const string CollectionName = "rome_statute";
        private const uint VectorSize = 3072;

        public string Collection => CollectionName;
        public string DistanceMetric => "Cosine";
        public uint ExpectedVectorSize => VectorSize;

        public QdrantVectorStoreService(IConfiguration config)
        {
            _client = new QdrantClient(
                host: config["Qdrant:Host"] ??
                    throw new InvalidOperationException("Qdrant Host is not configured. add it to appsettings.json"),
                port: 6334,
                apiKey: config["Qdrant:ApiKey"] ??
                    throw new InvalidOperationException("Qdrant ApiKey is not configured. add it to appsettings.json"),
                https: true
            );
        }

        public async Task EnsureCollectionAsync()
        {
            var collections = await _client.ListCollectionsAsync();
            if (collections.Any(c => c == CollectionName))
                return;

            await _client.CreateCollectionAsync(CollectionName,
                new VectorParams
                {
                    Size = VectorSize,
                    Distance = Distance.Cosine
                });
        }

        public async Task<bool> CollectionHasDataAsync()
        {
            try
            {
                var info = await _client.GetCollectionInfoAsync(CollectionName);
                return info.PointsCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task UpsertArticleAsync(RomeStatuteArticle article, float[] embedding)
        {
            var id = Guid.NewGuid().ToString();
            var point = new PointStruct
            {
                Id = new PointId { Uuid = id },
                Vectors = embedding,
                Payload =
                {
                    ["id"]             = id,
                    ["category"]       = article.Category,
                    ["article_number"] = article.ArticleNumber,
                    ["full_reference"] = article.FullReference,
                    ["title"]          = article.Title,
                    ["legal_text"]     = article.LegalText,
                    ["elements_of_crime"] = article.ElementsOfCrime,
                    ["contextual_requirement"] = article.ContextualRequirement,
                    ["conduct_examples"] = article.ConductExamples,
                    ["text_vectorized"]= article.TextToVectorize
                }
            };

            await _client.UpsertAsync(CollectionName, new[] { point });
        }

        public async Task<List<MatchedArticle>> SearchSimilarAsync(
            float[] queryEmbedding,
            int topK = 8)
        {
            var results = await _client.SearchAsync(
                CollectionName,
                queryEmbedding,
                limit: (ulong)topK,
                scoreThreshold: 0.55f       // ignore weak matches
            );

            return results.Select(r => new MatchedArticle
            {
                Category = r.Payload["category"].StringValue,
                ArticleNumber = r.Payload["article_number"].StringValue,
                FullReference = r.Payload["full_reference"].StringValue,
                Title = r.Payload["title"].StringValue,
                LegalText = r.Payload["legal_text"].StringValue,
                ElementsOfCrime = r.Payload["elements_of_crime"].StringValue,
                ContextualRequirement = r.Payload["contextual_requirement"].StringValue,
                ConductExamples = r.Payload["conduct_examples"].StringValue,
                TextToVectorize = r.Payload["text_vectorized"].StringValue,
                Score = r.Score
            }).ToList();
        }
    }
}
