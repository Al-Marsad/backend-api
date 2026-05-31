using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Models;
using BL.Models;

namespace BL.Services.Interfaces
{
    public interface IVectorStoreService<TArticle, TMatchedArticle>
    {
        public Task EnsureCollectionAsync();
        public Task<bool> CollectionHasDataAsync();

        public Task UpsertArticleAsync(TArticle article, float[] embedding);
        public Task<List<TMatchedArticle>> SearchSimilarAsync(
               float[] queryEmbedding,
               int topK = 8);


    }
}
