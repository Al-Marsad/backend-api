
namespace DAL.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        public Task<T> AddAsync(T obj);
        public Task UpdateAsync(T obj);
        public Task DeleteAsync(int id);
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<int> CountByUserIdAsync(int id);
        public Task<List<T>> GetPageAsync(int Skip, int Take, int userId);
        public Task<List<T>> GetByUserIdAsync(int id);
        public Task SaveAsync();
    }
}