using DAL.Entities;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface ICityRepository : ICreateRepository<City>, IGetAllRepository<City>,
        ISaveRepository
    {

    }
}
