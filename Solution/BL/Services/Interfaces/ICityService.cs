using BL.DTO.City;
using DAL.Entities;

namespace BL.Services.Interfaces
{
    public interface ICityService
    {
        public Task<ReturnCityDTO> AddAsync(AddCityDTO cityDTO); 
        public Task<List<ReturnCityDTO>> GetAllAsync();
    }
}
