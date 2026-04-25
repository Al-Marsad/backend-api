
namespace DAL.Repositories.Interfaces.Basic
{
    public interface ICountRepository
    {
        public Task<int> CountAsync();
    }
}
