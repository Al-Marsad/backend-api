using System.Security.Claims;
using BL.DTO.InitialIncidentReport;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}