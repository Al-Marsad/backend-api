using System.Security.Claims;
using BL.DTO.Evidence;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.DTO.InitialIncidentReport;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _incidentService;
        public IncidentController(IIncidentService incidentService,
            IIncidentRepository incidentRepository)
        {
            this._incidentService = incidentService;
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var data = await _incidentService.GetByIdAsync(Id);

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpPost]
        public async Task<IActionResult> AddIncident([FromBody] AddIncidentDTO incidentDTO)
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

            incidentDTO.FieldResearcherId = userId;

            var data = await _incidentService.AddAsync(incidentDTO);

            return StatusCode(201, new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpGet("Mine")]
        public async Task<IActionResult> GetFieldReseacherIncidentsByPage([FromQuery] PaginationDTO pageDTO, [FromQuery] string? NationalId,
            [FromQuery]bool OrderByDateOfOccurence = false, [FromQuery] bool DocumentationConsent = false,
            [FromQuery] bool PublicationConsent = false)
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

            var data = await _incidentService.GetFieldResearcherIncidentsByPageAsync(pageDTO, userId, NationalId, OrderByDateOfOccurence
                , DocumentationConsent, PublicationConsent);

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

        [Authorize(Roles = $"{RolesSelector.LegalTeamMember},{RolesSelector.Manager}")]
        [HttpGet()]
        public async Task<IActionResult> GetAllIncidentsByPage([FromQuery] PaginationDTO pageDTO, [FromQuery] int? CityId = null,
            [FromQuery] bool OrderByDateOfOccurence = false, [FromQuery]bool Approved = false,
            [FromQuery] int? Sensitivity = null)
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
            CityId ??= Convert.ToInt32(User.FindFirstValue("CityId"));
            Console.WriteLine(CityId);

            var data = await _incidentService.GetAllIncidentsByPageAsync(pageDTO, CityId, OrderByDateOfOccurence
                , Approved, Sensitivity);

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


        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpPost("{incidentId:int}/Evidences")]
        public async Task<IActionResult> AddIncidentRelatedEvidences([FromRoute] int incidentId,
            [FromForm] List<AddEvidenceDTO> Evidences)
        {

            var data = await _incidentService.AddRangeOfRelatedEvidences(Evidences, incidentId);

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }


        [Authorize(Roles = RolesSelector.FieldResearcher)]
        [HttpGet("{incidentId:int}/Evidences")]
        public async Task<IActionResult> GetEvidencesByIncidentId([FromRoute] int incidentId)
        {
            var data = await _incidentService.GetEvidencesByIncidentIdAsync(incidentId);
            
            return Ok(new
            {
                Success = true,
                Data = data
            });
        }


        [Authorize(Roles = $"{RolesSelector.FieldResearcher},{RolesSelector.LegalTeamMember}")]
        [HttpGet("{incidentId:int}/Testimonies")]
        public async Task<IActionResult> GetTestimoniesAndTheirVictimsByIncidentId([FromRoute] int incidentId)
        {
            var data = await _incidentService.GetTestimoniesAndTheirVictimsByIncidentIdAsync(incidentId);
            
            return Ok(new
            {
                Success = true,
                Data = data
            });

        }


        [Authorize(Roles = RolesSelector.LegalTeamMember)]
        [HttpPatch("{Id:int}/AssignToLegalTeamMember")]
        public async Task<IActionResult> AssignToLegalTeamMember([FromRoute] int Id)
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

            var data = await _incidentService.AssignToLegalTeamMember(userId, Id);

            return Ok(new
            {
                Success = true,
                Message = "Incident assigned to legal team member successfully",
                Data = data
            });
        }


        [Authorize(Roles = RolesSelector.LegalTeamMember)]
        [HttpPatch("{Id:int}/UnassignToLegalTeamMember")]
        public async Task<IActionResult> UnassignToLegalTeamMember([FromRoute] int Id)
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

            var data = await _incidentService.UnassignToLegalTeamMember(userId, Id);

            return Ok(new
            {
                Success = true,
                Message = "Incident unassigned from legal team member successfully",
                Data = data
            });
        }


        [Authorize(Roles = RolesSelector.LegalTeamMember)]
        [HttpPatch("{Id:int}")]
        public async Task<IActionResult> UpdateIncident([FromRoute] int Id, [FromBody] UpdateIncidentDTO updateIncidentDTO)
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

            var data = await _incidentService.UpdateIncident(updateIncidentDTO, Id, userId);

            return Ok(new
            {
                Success = true,
                Message = "Incident updated successfully",
                Data = data
            });
        }
    }
}