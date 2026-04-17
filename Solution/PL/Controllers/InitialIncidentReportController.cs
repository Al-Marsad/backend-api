using System.Data;
using System.Security.Claims;
using BL.DTO.InitialIncidentReport;
using BL.Helper;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            var currentUser = new CurrentUser
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Role = User.FindFirstValue(ClaimTypes.Role),
                CityId = User.FindFirstValue("CityId")
            };

            var data = await _initialReportService.GetByPageAsync(reportDto, currentUser);

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

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpPut("AssignToFieldResearcher/{Id:int}")]
        public async Task<IActionResult> AssignToFieldResearcher([FromRoute] int Id)
        {
            var dataDTO = new AssignToFieldResearcherDTO
            {
                ReportId = Id,
                FieldResearcherId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            var data = await _initialReportService.AssignToFieldResearcher(dataDTO);

            return Ok(new
            {
                Success = true,
                Message = "Initial report assigned to field researcher successfully",
                Data = data
            });
        }

    }
}