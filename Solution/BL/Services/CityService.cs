using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            if(city == null)
            {
                throw new DataNotFoundException("City not found");
            }

            _cityRepo.Delete(city);
            
            await _cityRepo.SaveAsync();
        }

    }
}
