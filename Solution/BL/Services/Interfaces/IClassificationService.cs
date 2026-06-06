
using BL.Models;

namespace BL.Services.Interfaces
{
    public interface IClassificationService<TInput, TMatchedArticle, TResult>
    {
        public Task<TResult> ClassifyAsync(
            TInput input,
            List<TMatchedArticle> matchedArticles);

        public Task<string> SendRequestAsync(string systemPrompt, string userPrompt);

    }
}
