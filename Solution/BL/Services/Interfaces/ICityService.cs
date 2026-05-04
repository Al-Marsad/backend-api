using BL.DTO.City;
using DAL.Entities;

namespace BL.Services.Interfaces
{
    public interface ICityService
    {
        public Task<ReturnCityDTO> AddAsync(AddCityDTO cityDTO); 
        public Task<List<ReturnCityDTO>> GetAllAsync(string? searchTerm = null);

        public Task DeleteAsync(int Id);
        public Task UpdateAsync(int Id, AddCityDTO cityDTO);

        public Task<int> CountAsync();
    }
}
