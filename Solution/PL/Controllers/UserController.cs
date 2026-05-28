using System.Security.Claims;
using BL.DTO.General;
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


        [Authorize(Roles = RolesSelector.Manager)]
        [HttpGet("Profile/{userId:required}")]
        public async Task<IActionResult> GetProfileByManager(string userId)
        {
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
        [HttpPut("{userId:required}")]
        public async Task<IActionResult> UpdateFullUserAccount(string userId, UpdateFullUserAccountDTO userDTO)
        {
            var currentUser = new CurrentUser
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Role = User.FindFirstValue(ClaimTypes.Role),
                CityId = User.FindFirstValue("CityId")
            };

            if (currentUser.UserId == null ||
                currentUser.CityId == null ||
                currentUser.Role == null)
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

            var data = await _userService.AdminUpdateUserAsync(userDTO, userId, currentUser);

            return Ok(new
            {
                Success = true,
                Message = "User information updated successfully",
                Data = data
            });
        }

        [Authorize]
        [HttpPatch("Password")]
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

        [Authorize(Roles = RolesSelector.Admin)]
        [HttpPatch("AccountStatus/{userId:required}")]
        public async Task<IActionResult> ChangeAccountStatus(string userId, ChangeAccountStatusDTO StatusDTO)
        {
            var currentUser = new CurrentUser
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Role = User.FindFirstValue(ClaimTypes.Role),
                CityId = User.FindFirstValue("CityId")
            };

            if (currentUser.UserId == null ||
                currentUser.CityId == null ||
                currentUser.Role == null)
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

            await _userService.ChangeAccountStatus(StatusDTO, userId, currentUser);

            return Ok(new
            {
                Success = true,
                Message = "Account status changed successfully",
            });

        }

        [Authorize(Roles = RolesSelector.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetUserByPage([FromQuery]PaginationDTO pageDTO, [FromQuery] UserNamesSearchDTO searchDTO)
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

            var data = await _userService.GetUsersByPageAsync(pageDTO, searchDTO, userId);

            return Ok(new
            {
                Success = true,
                Data = new
                {
                    Items = data.Data,
                    Pagination = new
                    {
                        CurrentPage = data.Page,
                        CurrentPageItems = data.Data.Count,
                        PageSize = data.PageSize,
                        TotalItems = data.TotalCount,
                    }
                }
            });
        }

        [Authorize(Roles = RolesSelector.Admin)]
        [HttpGet("UserCounts")]
        public async Task<IActionResult> GetUserCounts()
        {
            var data = await _userService.GetUserCountsAsync();

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize]
        [HttpGet("AccountStatuses")]
        public IActionResult GetAccountStatusValues()
        {
            var data = _userService.GetAccountStatusValues();
            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

    }
}
