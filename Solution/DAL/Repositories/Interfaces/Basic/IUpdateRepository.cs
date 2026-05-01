namespace DAL.Repositories.Interfaces.Basic
{
    public interface IUpdateRepository<T>
    {
        public Task UpdateAsync(T entity);
    }
}
