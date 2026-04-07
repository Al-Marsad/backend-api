
namespace DAL.Repositories.Interfaces.Basic
{
    public interface IGetAllRepository<T>
    {
        public Task<List<T>> GetAllAsync();

    }
}
