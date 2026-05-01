namespace DAL.Repositories.Interfaces.Basic
{
    public interface IUpdateRepository<T>
    {
        public void Update(T entity);
    }
}
