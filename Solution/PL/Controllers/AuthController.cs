using BL.DTO.User;
using BL.Services;
using BL.Services.Interfaces;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO userDTO)
        {
            var data = await _authService.Login(userDTO);

            Response.Cookies.Append("RefreshToken", data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = data.RefreshTokenExpirationTime
            });

            return Ok(new
            {
                Success = true,
                Message = "Logged in successfully",
                Data = data
            });
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] RefreshTokenDTO? refreshTokenDTO = null)
        {
            var usedToken = !string.IsNullOrEmpty(refreshTokenDTO?.RefreshToken)
                  ? refreshTokenDTO.RefreshToken
                  : Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(usedToken))
                throw new UnauthorizedException("No refresh token provided");
            
            var data = await _authService.Refresh(usedToken);

            Response.Cookies.Append("RefreshToken", data.RefreshToken, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = data.RefreshTokenExpirationTime

            });

            return Ok(new
            {
                Success = true,
                Message = "Refreshed successfully",
                Data = data
            });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] RefreshTokenDTO? refreshTokenDTO = null)
        {
            var usedToken = !string.IsNullOrEmpty(refreshTokenDTO?.RefreshToken)
                 ? refreshTokenDTO.RefreshToken
                 : Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(usedToken))
                throw new UnauthorizedException("No refresh token provided");

            await _authService.Logout(usedToken);

            Response.Cookies.Delete("RefreshToken");

            return NoContent();
        }


    }
}
