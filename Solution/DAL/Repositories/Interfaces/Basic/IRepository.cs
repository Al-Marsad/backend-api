using DAL.Entities;

namespace DAL.Repositories.Interfaces.Basic
{
    public interface IRepository<T> : ICreateRepository<T>,
        ISaveRepository,
        IGetByIdRepository<T>,
        IGetPageRepository<T>,
        IGetAllRepository<T>
    {
        public Task UpdateAsync(T obj);
        public Task DeleteAsync(int id);
        public Task<int> CountByUserIdAsync(int id);
        public Task<List<T>> GetByUserIdAsync(int id);
    }
}