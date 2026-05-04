using AutoMapper;
using BL.DTO.City;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepo;
        private readonly IMapper _mapper;
        public CityService(ICityRepository cityRepo,
            IMapper mapper)
        {
            this._cityRepo = cityRepo;
            this._mapper = mapper;
        }
        public async Task<ReturnCityDTO> AddAsync(AddCityDTO cityDTO)
        {
            var city = _mapper.Map<City>(cityDTO);

            await _cityRepo.AddAsync(city);

            await _cityRepo.SaveAsync();

            return _mapper.Map<ReturnCityDTO>(city);
        }
        public async Task<List<ReturnCityDTO>> GetAllAsync(string? searchTerm = null)
        {
            return _mapper.Map<List<ReturnCityDTO>>(await _cityRepo.GetAllAsync(searchTerm));
        }
        public async Task DeleteAsync(int Id)
        {
            var city = await _cityRepo.GetByIdAsync(Id);

            if (city == null)
            {
                throw new DataNotFoundException("There is no city found with this id");
            }

            _cityRepo.Delete(city);

            await _cityRepo.SaveAsync();
        }

        public async Task UpdateAsync(int Id, AddCityDTO cityDTO)
        {
            var city = await _cityRepo.GetByIdAsync(Id);

            if (city == null)
            {
                throw new DataNotFoundException("There is no city found with this id to update");
            }

            city.ArabicName = cityDTO.ArabicName ?? city.ArabicName;
            city.EnglishName = cityDTO.EnglishName ?? city.EnglishName;

            _cityRepo.Update(city);

            await _cityRepo.SaveAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _cityRepo.CountAsync();
        }
    }
}