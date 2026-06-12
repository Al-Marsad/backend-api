using System.Security.Claims;
using BL.DTO.General;
using BL.DTO.News;
using BL.Helper;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsItemController : ControllerBase
    {
        private readonly INewsItemService _newsItemService;

        public NewsItemController(INewsItemService newsItemService)
        {
            _newsItemService = newsItemService;
        }

        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpPost("Management")]
        public async Task<IActionResult> AddNewsItem([FromBody] AddNewsItemDTO newsDTO)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return UnauthorizedResponse();

            var data = await _newsItemService.AddAsync(newsDTO, currentUser);

            return StatusCode(201, new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpGet("Management")]
        public async Task<IActionResult> GetNewsForManagement([FromQuery] PaginationDTO pageDTO,
            [FromQuery] string? Search = null,
            [FromQuery] int? CityId = null,
            [FromQuery] bool? IsPublished = null)
        {
            var data = await _newsItemService.GetManagementByPageAsync(pageDTO, Search, null, CityId,
                IsPublished);

            return OkPaged(data);
        }

        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpGet("Management/Mine")]
        public async Task<IActionResult> GetNewsByWrittenById(
            [FromQuery] PaginationDTO pageDTO, [FromQuery] string? Search = null,
            [FromQuery] int? CityId = null, [FromQuery] bool? IsPublished = null)
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

            var data = await _newsItemService.GetByWrittenByIdAsync(pageDTO, userId, Search, CityId, IsPublished);

            return OkPaged(data);
        }

        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpGet("Management/{newsId:int}")]
        public async Task<IActionResult> GetNewsForManagementById([FromRoute] int newsId)
        {
            var data = await _newsItemService.GetManagementByIdAsync(newsId);

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpGet("Stats")]
        public async Task<IActionResult> GetStats()
        {
            var data = await _newsItemService.GetStatsAsync();

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpPut("Management/{newsId:int}")]
        public async Task<IActionResult> UpdateNewsItem([FromRoute] int newsId, [FromBody] UpdateNewsItemDTO newsDTO)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return UnauthorizedResponse();

            var data = await _newsItemService.UpdateAsync(newsId, newsDTO, currentUser);

            return Ok(new
            {
                Success = true,
                Message = "News item updated successfully",
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpPatch("Management/{newsId:int}/Publish")]
        public async Task<IActionResult> PublishNewsItem([FromRoute] int newsId)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return UnauthorizedResponse();

            var data = await _newsItemService.PublishAsync(newsId, currentUser);

            return Ok(new
            {
                Success = true,
                Message = "News item published successfully",
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpPatch("Management/{newsId:int}/Unpublish")]
        public async Task<IActionResult> UnpublishNewsItem([FromRoute] int newsId)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return UnauthorizedResponse();

            var data = await _newsItemService.UnpublishAsync(newsId, currentUser);

            return Ok(new
            {
                Success = true,
                Message = "News item unpublished successfully",
                Data = data
            });
        }

        [HttpGet("Website")]
        public async Task<IActionResult> GetAbbreviatedNewsForWebsite([FromQuery] PaginationDTO pageDTO,
            [FromQuery] string? Search = null, [FromQuery] int? CityId = null)
        {
            var data = await _newsItemService.GetWebsiteAbbreviatedNewsAsync(pageDTO, Search, CityId);

            return OkPaged(data);
        }

        [HttpGet("Website/Map")]
        public async Task<IActionResult> GetNewsMapForWebsite([FromQuery] PaginationDTO pageDTO,
            [FromQuery] string? Search = null, [FromQuery] int? CityId = null)
        {
            var data = await _newsItemService.GetWebsiteMapNewsAsync(pageDTO, Search, CityId);

            return OkPaged(data);
        }

        [HttpGet("Website/{newsId:int}")]
        public async Task<IActionResult> GetNewsForWebsiteById([FromRoute] int newsId)
        {
            var data = await _newsItemService.GetWebsiteByIdAsync(newsId);

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        private CurrentUser? GetCurrentUser()
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
                return null;
            }

            return currentUser;
        }

        private OkObjectResult OkPaged<T>(PagedResultDTO<List<T>> data)
        {
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

        private UnauthorizedObjectResult UnauthorizedResponse()
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
    }
}
