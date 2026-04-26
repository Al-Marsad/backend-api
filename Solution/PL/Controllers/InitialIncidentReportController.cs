using System.Data;
using System.Security.Claims;
using BL.DTO.General;
using BL.DTO.InitialIncidentReport;
using BL.Helper;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

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

            var dataDTO = new AssignToFieldResearcherDTO
            {
                ReportId = Id,
                FieldResearcherId = userId
            };

            var data = await _initialReportService.AssignToFieldResearcher(dataDTO);

            return Ok(new
            {
                Success = true,
                Message = "Initial report assigned to field researcher successfully",
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpPut("UnassignToFieldResearcher/{Id:int}")]
        public async Task<IActionResult> UnassignToFieldResearcher([FromRoute] int Id)
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

            var dataDTO = new AssignToFieldResearcherDTO
            {
                ReportId = Id,
                FieldResearcherId = userId
            };

            var data = await _initialReportService.UnassignToFieldResearcher(dataDTO);

            return Ok(new
            {
                Success = true,
                Message = "Receiving successfully cancelled.",
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpGet("MyAssigned")]
        public async Task<IActionResult> GetFieldResearcherAssignedReports([FromQuery]PaginationDTO pageDTO, 
            [FromQuery]string? search = null)
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

            var data = await _initialReportService.GetMyAssignedReportsAsync(userId, pageDTO, search);
            
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