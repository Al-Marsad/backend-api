using DAL.Entities;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IVictimRepository /*IGetByIdRepository<Victim>*/
    {
        public Task<Victim?> GetByNationalIdAsync(string nationalId);

    }
}
