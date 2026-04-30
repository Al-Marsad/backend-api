using DAL.DBContext;
using DAL.Entities;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<List<City>> GetAllAsync(string? searchTerm = null)
        {
            var query = _dbContext.Cities.AsQueryable();

            if(!String.IsNullOrEmpty(searchTerm))
                query = query.Where(c => c.ArabicName.Contains(searchTerm) || c.EnglishName.Contains(searchTerm));
            

            return await query.ToListAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            var city = await _dbContext.Cities.SingleOrDefaultAsync(c => c.Id == Id);
            
            if (city == null)
            {
                throw new DataNotFoundException("There is no city found with this id");
            }

            _dbContext.Cities.Remove(city);
        }
    }
}