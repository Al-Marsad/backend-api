using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CityRepository : ICityRepository
    {
        private AlMarsadDbContext _dbContext;

        public CityRepository(AlMarsadDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddAsync(City city)
        {
            await _dbContext.Cities.AddAsync(city);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();    
        }

        public async Task<List<City>> GetAllAsync()
        {
            return await _dbContext.Cities.ToListAsync(); 
        }
    }
}
