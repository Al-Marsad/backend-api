using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BL.DTO.City;
using BL.Services.Interfaces;
using DAL.Entities;
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
        public async Task<List<ReturnCityDTO>> GetAllAsync()
        {
            return _mapper.Map<List<ReturnCityDTO>>(await _cityRepo.GetAllAsync());
        }
    }
}
