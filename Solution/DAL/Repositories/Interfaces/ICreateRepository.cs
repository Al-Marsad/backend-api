
namespace DAL.Repositories.Interfaces
{
    public interface ICreateRepository<T>
    {
        public Task AddAsync(T obj);
    }
}