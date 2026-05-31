using System.Net.Http.Json;
using System.Text.Json.Serialization;
using BL.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BL.Services
{
    public class GeminiEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private const string Model = "gemini-embedding-001";
        public string ModelName => Model;   

        public GeminiEmbeddingService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["Gemini:ApiKey"] ??
                throw new InvalidOperationException("Gemini ApiKey is not configured. add it to appsettings.json");
        }


        public async Task<float[]> EmbedAsync(string text)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:embedContent?key={_apiKey}";

            var body = new
            {
                model = $"models/{Model}",
                content = new
                {
                    parts = new[] { new { text } }
                }
            };

            var response = await _http.PostAsJsonAsync(url, body);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Gemini embedding failed ({response.StatusCode}): {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<GeminiEmbedResponse>()
                ?? throw new InvalidOperationException("Empty response from Gemini.");

            return result.Embedding.Values;
        }
    }

    file record GeminiEmbedResponse(
        [property: JsonPropertyName("embedding")] GeminiEmbedding Embedding);

    file record GeminiEmbedding(
        [property: JsonPropertyName("values")] float[] Values);


}
