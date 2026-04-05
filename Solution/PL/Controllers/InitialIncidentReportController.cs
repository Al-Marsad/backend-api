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

        public InitialIncidentReportController(IInitialIncidentReportService initialReportService) { 
        
            this._initialReportService = initialReportService;
        }
        
        [HttpPost]
        public async Task<IActionResult> SendReport(AddInitialIncidentReportDTO reportDto)
        {

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