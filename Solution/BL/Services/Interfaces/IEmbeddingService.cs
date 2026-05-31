namespace BL.Services.Interfaces
{
    public interface IEmbeddingService
    {
        public Task<float[]> EmbedAsync(string text);

    }
}
