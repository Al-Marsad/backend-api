using BL.DTO.General;
using BL.Models;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ISeeder<RomeStatuteArticle> _seeder;

        public SeedController(ISeeder<RomeStatuteArticle> seeder) => _seeder = seeder;

        [HttpPost("RomeStatute")]
        public async Task<IActionResult> Seed([FromBody]SeedVectorDatabaseDTO data)
        {
            var result = await _seeder.SeedAsync(data.Articles);

            if (result.Failed > 0)
                return StatusCode(207, new
                {
                    message = $"Seeding partially complete.",
                    success = result.Success,
                    failed = result.Failed
                });

            return Ok(new
            {
                message = $"Successfully seeded {result.Success} Rome Statute articles.",
                success = result.Success
            });
        }
    }
}
