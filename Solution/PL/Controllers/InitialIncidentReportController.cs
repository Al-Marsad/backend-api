using System.Security.Claims;
using BL.DTO.InitialIncidentReport;
using BL.Services.Interfaces;
using DAL.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InitialIncidentReportController : ControllerBase
    {
        private readonly IInitialIncidentReportService _initialReportService;

        public InitialIncidentReportController(IInitialIncidentReportService initialReportService) { 
        
            this._initialReportService = initialReportService;
        }

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

            return StatusCode(201, new
            {
                Success = true,
                Message = "Initial report added successfully",
                Data = data            
            });
        }

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

            var data = new List<ReturnDetailedInitialIncidentReportDTO>();

            if (reportDto.Status == null)
            {
                data = await _initialReportService.GetByPageAsync(reportDto.Page, reportDto.PageSize, userId);
            } else
            {
                data = await _initialReportService.GetByPageAsync(reportDto.Page, reportDto.PageSize, userId, reportDto.Status.Value);

            }

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


    }
}