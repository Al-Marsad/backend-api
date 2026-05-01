using BL.DTO.City;
using BL.Helper;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> GetAll([FromQuery]string? Search)
        {
            var data = await _cityService.GetAllAsync(Search);
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

        [Authorize(Roles = RolesSelector.Admin)]
        [HttpDelete("{CityId}")]
        public async Task<IActionResult> Delete([FromRoute]int CityId)
        {
            await _cityService.DeleteAsync(CityId);

            return Ok(new
            {
                Success = true,
                Message = "City Deleted Successfully"
            });
        }

        [Authorize(Roles = RolesSelector.Admin)]
        [HttpPut("{CityId}")]
        public async Task<IActionResult> Update([FromRoute] int CityId, [FromBody]AddCityDTO cityDTO)
        {
            await _cityService.UpdateAsync(CityId, cityDTO);

            return Ok(new
            {
                Success = true,
                Message = "City Updated Successfully"
            });
        }

    }
}
