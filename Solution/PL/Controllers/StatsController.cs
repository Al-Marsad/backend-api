using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("Public")]
        public async Task<IActionResult> GetPublicStats()
        {
            var data = await _statsService.GetPublicStatsAsync();

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [HttpGet("Analytics")]
        public async Task<IActionResult> GetAnalytics()
        {
            var data = await _statsService.GetAnalyticsAsync();

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }
    }
}
