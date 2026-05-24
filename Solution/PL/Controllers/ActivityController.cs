using BL.DTO.General;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        public ActivityController(IActivityService activityService)
        {
            this._activityService = activityService;
        }

        [Authorize(Roles = RolesSelector.Manager)]
        [HttpGet]
        public async Task<IActionResult> GetByPageAsync([FromQuery] PaginationDTO pageDTO, [FromQuery] string? SearchContent = null
            , [FromQuery] ActivityType? ActivityType = null)
        {
            var data = await _activityService.GetByPageAsync(pageDTO, SearchContent, ActivityType);

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
    }
}
