namespace DAL.Repositories.Interfaces.Basic
{
    public interface ICreateRepository<T>
    {
        public Task AddAsync(T obj);
    }
}