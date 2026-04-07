using BL.DTO.City;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL.Helper;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _cityService.GetAllAsync();
            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.Admin)]
        [HttpPost]
        public async Task<IActionResult> Add(AddCityDTO cityDTO)
        {
            var data = await _cityService.AddAsync(cityDTO);

            return StatusCode(201, new
            {
                Success = true,
                Data = data
            });
        }
    }
}
