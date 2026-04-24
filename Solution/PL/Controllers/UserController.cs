using System.Security.Claims;
using BL.DTO.User;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
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

        [Authorize]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileDTO profileDTO)
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

            var data = await _userService.UpdateProfileAsync(profileDTO, userId);
            
            return Ok(new
            {
                Success = true,
                Message = "Profile updated successfully",   
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.Admin)]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateFullUserAccount(string userId, UpdateFullUserAccountDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = new
                    {
                        Code = "BAD_REQUEST",
                        Message = "User ID is required in the route."
                    }
                });
            }

            var data = await _userService.AdminUpdateUserAsync(userDTO, userId);

            return Ok(new
            {
                Success = true,
                Message = "User information updated successfully",
                Data = data
            });
        }

        [Authorize]
        [HttpPut("Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO passwordDTO)
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

            await _userService.ChangePasswordAsync(passwordDTO, userId);

            return Ok(new
            {
                Success = true,
                Message = "Password changed successfully",
            });
        }

    }
}
