using BL.DTO.Incident;
using BL.DTO.Victim;
using BL.Helper;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VictimController : ControllerBase
    {
        private readonly IVictimService _victimService;
        public VictimController(IVictimService victimService)
        {
            this._victimService = victimService;
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpGet("Exists/{NationalId}")]
        public async Task<IActionResult> CheckVictimExists(string NationalId)
        {
            var exists = await _victimService.VictimExists(NationalId);
            return Ok(new
            {
                Success = true,
                Data = new
                {
                    Exists = exists
                }
            });
        }

    }
}
