using System.Security.Claims;
using BL.DTO.InitialIncidentReport;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BL.Helper;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitialIncidentReportController : ControllerBase
    {
        private readonly IInitialIncidentReportService _initialReportService;

        public InitialIncidentReportController(IInitialIncidentReportService initialReportService) { 
        
            this._initialReportService = initialReportService;
        }

        [Authorize(Roles = RolesSelector.Citizen)]
        [HttpPost]
        public async Task<IActionResult> SendReport(AddInitialIncidentReportDTO reportDto)
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
            
            reportDto.CitizenReporterId = userId;
            var data = await this._initialReportService.AddAsync(reportDto);

            return CreatedAtAction(nameof(GetById), new { Id = data.Id }, new
            {
                Success = true,
                Message = "Initial report added successfully",
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.FieldResearcher},{RolesSelector.Citizen}")]
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
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

            var data = await _initialReportService.GetByIdAsync(Id, userId);

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.FieldResearcher},{RolesSelector.Citizen}")]
        [HttpGet("Mine")]
        public async Task<IActionResult> GetByPage([FromQuery]GetByPageInitialIncidentReportDTO reportDto)
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

            var data = await _initialReportService.GetByPageAsync(reportDto.Page, reportDto.PageSize, userId, reportDto.Status, reportDto.CityId);

            return Ok(new
            {
                Success = true,
                Data = new
                {
                    Items = data,
                    Pagination = new
                    {
                        CurrentPage = reportDto.Page,
                        PageSize = reportDto.PageSize,
                        TotalItems = data.Count,
                    }
                }
            });
        }

        [Authorize(Roles = $"{RolesSelector.FieldResearcher},{RolesSelector.Citizen}")]
        [HttpGet("Statuses")]
        public IActionResult GetStatusValues()
        {
            var data = _initialReportService.GetStatusValues();
            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

    }
}