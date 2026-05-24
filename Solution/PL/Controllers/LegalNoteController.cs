using System.Security.Claims;
using BL.DTO.LegalNote;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalNoteController : ControllerBase
    {
        private readonly ILegalNoteService _legalNoteService;
        public LegalNoteController(ILegalNoteService legalNoteService)
        {
            _legalNoteService = legalNoteService;
        }

        [Authorize(Roles = $"{RolesSelector.LegalTeamMember}")]
        [HttpPost]
        public async Task<IActionResult> AddLegalNote([FromBody] AddLegalNoteDTO noteDTO)
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

            var data = await _legalNoteService.AddAsync(userId, noteDTO);

            return StatusCode(201, new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.LegalTeamMember}")]
        [HttpPut("{noteId:int}")]
        public async Task<IActionResult> UpdateLegalNote([FromRoute]int noteId, [FromBody] UpdateLegalNoteDTO noteDTO)
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

            var data = await _legalNoteService.UpdateAsync(userId, noteId, noteDTO);

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.LegalTeamMember}")]
        [HttpDelete("{noteId:int}")]
        public async Task<IActionResult> DeleteLegalNote([FromRoute] int noteId)
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

            await _legalNoteService.DeleteAsync(userId, noteId);

            return NoContent();
        }

    }
}
