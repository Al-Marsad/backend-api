using System.Security.Claims;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _incidentService;
        public IncidentController(IIncidentService incidentService,
            IIncidentRepository incidentRepository)
        {
            this._incidentService = incidentService;
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpPost]
        public async Task<IActionResult> AddIncident([FromBody] AddIncidentDTO incidentDTO)
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

            incidentDTO.FieldResearcherId = userId;

            var data = await _incidentService.AddAsync(incidentDTO);

            return StatusCode(201, new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpGet("Mine")]
        public async Task<IActionResult> GetByPage([FromQuery]PaginationDTO pageDTO, [FromQuery]string? NationalId)
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

            var data = await _incidentService.GetByPageAsync(pageDTO, userId, NationalId);

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
