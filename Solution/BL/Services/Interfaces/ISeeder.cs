
using BL.Models;
using BL.Services;

namespace BL.Services.Interfaces
{
    public interface ISeeder<TArticle>
    {
        public Task<SeedResult> SeedAsync(List<TArticle> articles);

    }
}
