using BL.DTO.User;
using BL.Services;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PL.Helper;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register/Citizen")]
        public async Task<IActionResult> RegisterCitizen(AddUserDTO userDTO)
        {
            var data = await _authService.Regsiter(userDTO, RolesSelector.Citizen);

            return StatusCode(201, new
            {
                Success = true,
                Message = "Account created successfully",
                Data = data
            });
        }
    }
}
