using System.Security.Claims;
using BL.DTO.User;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Error = new
                    {
                        Code = "UNAUTHORIZED",
                        Message = "JWT missing or expired !!"
                    }
                });
            }

            var data = await _userService.GetProfileAsync(userId);
            
            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileDTO profileDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = await _userService.UpdateProfileAsync(profileDTO, userId);
            
            return Ok(new
            {
                Success = true,
                Message = "Profile updated successfully",   
                Data = data
            });
        }
    }
}
