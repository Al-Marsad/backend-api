using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class VictimRepository : IVictimRepository
    {
        private readonly AlMarsadDbContext _dbContext;

        public VictimRepository(AlMarsadDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Victim?> GetByNationalIdAsync(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                return null;

            var victim = await this._dbContext.Victims.SingleOrDefaultAsync(v => v.NationalId == nationalId);

            return victim;
        }

    }
}
