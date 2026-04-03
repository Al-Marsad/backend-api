using BL.DTO.InitialIncidentReport;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.Helper;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitialIncidentReportController : ControllerBase
    {
        private readonly IInitialIncidentReportService _initialReportService;
        private readonly UserManager<AppUser> _userManager;

        public InitialIncidentReportController(IInitialIncidentReportService initialReportService, UserManager<AppUser> userManager) { 
        
            this._initialReportService = initialReportService;
            this._userManager = userManager;
        }
        
        [HttpPost("Send")]
        public async Task<IActionResult> SendReport(AddInitialIncidentReportDTO reportDto)
        {

            var data = await this._initialReportService.AddAsync(reportDto);

            return StatusCode(201, new
            {
                Success = true,
                Message = "Initial Report Added Successfully",
                Data = data            
            });
        }
    }
}