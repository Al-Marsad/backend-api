using System.Security.Claims;
using BL.DTO.Evidence;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.DTO.InitialIncidentReport;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
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
        private readonly ILegalNoteService _legalNoteService;

        public IncidentController(IIncidentService incidentService,
            ILegalNoteService _legalNoteService)
        {
            this._incidentService = incidentService;
            this._legalNoteService = _legalNoteService;
        }

        [Authorize(Roles = RolesSelector.Manager)]
        [HttpGet("Stats")]
        public async Task<IActionResult> GetStats()
        {
            var data = await _incidentService.GetStatsAsync();

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.LegalTeamMember)]
        [HttpGet("MyStats")]
        public async Task<IActionResult> GetMyStats()
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

            var data = await _incidentService.GetMyStatsAsync(userId);

            return Ok(new
            {
                Success = true,
                Data = data
            });
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

        [Authorize(Roles = $"{RolesSelector.FieldResearcher},{RolesSelector.LegalTeamMember}")]
        [HttpGet("Mine")]
        public async Task<IActionResult> GetIncidentsByPageByUser([FromQuery] PaginationDTO pageDTO, [FromQuery]int? CityId 
            ,[FromQuery] string? NationalId = null, [FromQuery]bool OrderByDateOfOccurence = false, [FromQuery] bool? DocumentationConsent = null,
            [FromQuery] bool? PublicationConsent = null, [FromQuery] bool? PreventModification = null, [FromQuery]int? Sensitivity = null)
        {
            var currentUser = new CurrentUser
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Role = User.FindFirstValue(ClaimTypes.Role),
                CityId = User.FindFirstValue("CityId")
            };

            if (currentUser.UserId == null ||
                currentUser.CityId == null ||
                currentUser.Role == null)
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

            var data = await _incidentService.GetIncidentsByPageAndUserIdAsync(pageDTO, currentUser, CityId, NationalId, OrderByDateOfOccurence
                , DocumentationConsent, PublicationConsent, PreventModification, Sensitivity);

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
        [HttpGet]
        public async Task<IActionResult> GetAllIncidentsByPage([FromQuery] PaginationDTO pageDTO, [FromQuery] int? CityId = null,
            [FromQuery] string? NationalId = null, [FromQuery] bool OrderByDateOfOccurence = false, [FromQuery] bool? DocumentationConsent = null,
            [FromQuery] bool? PublicationConsent = null,
            [FromQuery] bool? PreventModification = null, [FromQuery]int? Sensitivity = null)
        {
            CityId ??= Convert.ToInt32(User.FindFirstValue("CityId"));
            Console.WriteLine(CityId);

            var data = await _incidentService.GetAllIncidentsByPageAsync(pageDTO, CityId,NationalId, OrderByDateOfOccurence
                , DocumentationConsent, PublicationConsent,PreventModification, Sensitivity);

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


        [Authorize(Roles = $"{RolesSelector.FieldResearcher},{RolesSelector.LegalTeamMember},{RolesSelector.Manager}")]
        [HttpGet("{incidentId:int}/Evidences")]
        public async Task<IActionResult> GetEvidencesByIncidentId([FromRoute] int incidentId, [FromQuery]EvidenceType? Type = null)
        {
            var data = await _incidentService.GetEvidencesByIncidentIdAsync(incidentId, Type);
            
            return Ok(new
            {
                Success = true,
                Data = data
            });
        }


        [Authorize(Roles = $"{RolesSelector.FieldResearcher},{RolesSelector.LegalTeamMember},{RolesSelector.Manager}")]
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

        [Authorize(Roles = RolesSelector.LegalTeamMember)]
        [HttpPatch("{Id:int}/GiveDocumentationConsent")]
        public async Task<IActionResult> GiveDocumentationConsent([FromRoute] int Id)
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

            var data = await _incidentService.GiveDocumentationConsentAsync(Id, userId);

            return Ok(new
            {
                Success = true,
                Message = "Incident had documentation consent successfully",
                Data = data
            });
        }

        [Authorize(Roles = RolesSelector.Manager)]
        [HttpPatch("{Id:int}/GivePublicationConsent")]
        public async Task<IActionResult> GivePublicationConsent([FromRoute] int Id)
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

            var data = await _incidentService.GivePublicationConsentAsync(Id, userId);

            return Ok(new
            {
                Success = true,
                Message = "Incident had publication consent successfully",
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.LegalTeamMember},{RolesSelector.Manager}")]
        [HttpGet("{IncidentId:int}/LegalNote")]
        public async Task<IActionResult> GetLegalNoteForIncident([FromRoute] int IncidentId)
        {
            var data = await _legalNoteService.GetByIncident(IncidentId);

            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        [Authorize(Roles = $"{RolesSelector.LegalTeamMember}")]
        [HttpPost("{IncidentId:int}/RequestModification")]
        public async Task<IActionResult> RequestModification([FromRoute] int IncidentId)
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

            await _incidentService.RequestModificationAsync(IncidentId, userId);

            return StatusCode(201, new
            {
                Success = true,
                Message = "Modification request submitted successfully"
            });
        }


        [Authorize(Roles = $"{RolesSelector.Manager}")]
        [HttpPatch("{IncidentId:int}/AllowModification")]
        public async Task<IActionResult> AllowModification([FromRoute] int IncidentId)
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

            var data = await _incidentService.AllowModificationAsync(IncidentId, userId);

            return Ok(new
            {
                Success = true,
                Message = "Modification request submitted successfully",
                Data = data
            });
        }
    }
}
