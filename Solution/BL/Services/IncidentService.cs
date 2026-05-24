using AutoMapper;
using BL.DTO.Evidence;
using BL.DTO.General;
using BL.DTO.Incident;
using BL.DTO.Victim;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly IIncidentRepository _incidentRepo;
        private readonly IVictimRepository _victimRepo;
        private readonly IInitialIncidentReportRepository _initialIncidentReportRepo; 
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IActivityRepositoy _activityRepositoy;

        public IncidentService(IIncidentRepository incidentRepo,
            IVictimRepository victimRepo,
            IMapper mapper,
            IInitialIncidentReportRepository initialIncidentReportRepo,
            ICloudinaryService cloudinaryService,
            IActivityRepositoy activityRepositoy)

        {
            _incidentRepo = incidentRepo;
            _victimRepo = victimRepo;
            _mapper = mapper;
            _initialIncidentReportRepo = initialIncidentReportRepo;
            _cloudinaryService = cloudinaryService;
            _activityRepositoy = activityRepositoy;
        }

        public async Task<ReturnIncidentDTO> GetByIdAsync(int Id)
        {
            var incident = await _incidentRepo.GetByIdAsync(Id);

            if(incident == null)
                throw new DataNotFoundException($"Incident with id '{Id}' not found");

            return _mapper.Map<ReturnIncidentDTO>(incident);    
        }

        public async Task<ReturnAssignedIncidentDTO> AssignToLegalTeamMember(string userId, int IncidentId)
        {
            var incident = await _incidentRepo.GetByIdAsync(IncidentId);

            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{IncidentId}' not found");

            if(incident.PreventModification)
                throw new ConflictException($"Incident with id '{IncidentId}' cannot be assigned," +
                    $"You need permission from the manager");

            if(incident.LegalTeamMemberId != null)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' is already assigned " +
                    $"to legal team member with id '{incident.LegalTeamMemberId}'");
            }

            if(incident.DocumentationConsent)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' is already documented");
            }

            if (incident.PublicationConsent)
            {
                throw new ConflictException($"you can't Modify this incident with id '{IncidentId}', it have been already published");
            }

            incident.LegalTeamMemberId = userId;


            await _activityRepositoy.AddAsync(new Activity
            {
                Description = $"User with id '{userId}' assigned themselves to incident with id '{IncidentId}'",
                MadeById = userId,
                Type = ActivityType.Update,

            }); 

            await _incidentRepo.SaveAsync();

            return _mapper.Map<ReturnAssignedIncidentDTO>(incident);
        }

        public async Task<ReturnAssignedIncidentDTO> UnassignToLegalTeamMember(string userId, int IncidentId)
        {
            var incident = await _incidentRepo.GetByIdAsync(IncidentId);

            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{IncidentId}' not found");

            if (incident.PreventModification)
                throw new ConflictException($"Incident with id '{IncidentId}' cannot be unassigned," +
                    $"You need permission from the manager");

            if (incident.LegalTeamMemberId == null)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' is not assigned " +
                    $"to any legal team member");
            }

            if (incident.DocumentationConsent)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' is already documented");
            }

            if (incident.PublicationConsent)
            {
                throw new ConflictException($"you can't Modify this incident with id '{IncidentId}', it have been already published");
            }

            incident.LegalTeamMemberId = null;


            await _activityRepositoy.AddAsync(new Activity
            {
                Description = $"User with id '{userId}' unassigned themselves from incident with id '{IncidentId}'",
                MadeById = userId,
                Type = ActivityType.Update,

            });

            await _incidentRepo.SaveAsync();

            return _mapper.Map<ReturnAssignedIncidentDTO>(incident);
        }

        public async Task<ReturnFullIncidentDTO> AddAsync(AddIncidentDTO incidentDTO)
        {
            InitialIncidentReport? intiaIncident = null;
            if (incidentDTO.InitialIncidentReportId.HasValue)
            {
                intiaIncident = await _initialIncidentReportRepo.GetByIdAsync(incidentDTO.InitialIncidentReportId.Value);
                if(intiaIncident == null)
                {
                    throw new DataNotFoundException($"Initial Incident report with ;{incidentDTO.InitialIncidentReportId.Value}' id is not found");
                } else
                {
                    if(intiaIncident.Status != InitialIncidentReportStatus.ASSIGNED)
                    {
                        throw new ConflictException($"You can't create a full report for initial incident report with status = {intiaIncident.Status}");
                    }

                    if(intiaIncident.FieldResearcherId != incidentDTO.FieldResearcherId)
                    {
                        throw new ForbiddenException($"You are not allowed to create full report for this initial incident report");
                    }

                    var hasIncident = await _initialIncidentReportRepo.HasIncident(incidentDTO.InitialIncidentReportId.Value);
                    if(hasIncident)
                    {
                        throw new ConflictException("Initial Incident Report has already Full report, You can't create new one", new
                        {
                            InitialIncidentReportId = "It belongs to another full report"
                        });
                    }
                }
            }


            if (incidentDTO.PersonalVictimTestimonies != null)
            {
                foreach (var testimonie in incidentDTO.PersonalVictimTestimonies)
                {
                    if (testimonie.NationalId != null && testimonie.Victim != null)
                    {
                        throw new ValidationException("Provide either NationalId or VictimDTO, not both");
                    }

                    if (testimonie.NationalId == null && testimonie.Victim == null)
                    {
                        throw new ValidationException("One of NationalId or VictimDTO must be provided");
                    }

                    if (testimonie.NationalId != null)
                    {
                        var victim = await _victimRepo.GetByNationalIdAsync(testimonie.NationalId);

                        if (victim == null)
                            throw new DataNotFoundException($"Victim with '{testimonie.NationalId}' national id not found");
                        else
                            testimonie.VictimId = victim.Id;
                    }

                    if (testimonie.Victim != null)
                    {
                        if (!Enum.IsDefined(typeof(MaritalStatus), testimonie.Victim.MaritalStatus))
                        {
                            throw new ValidationException($"Invalid Marital Status value: {testimonie.Victim.MaritalStatus}");
                        }

                        if (!Enum.IsDefined(typeof(Gender), testimonie.Victim.Gender))
                        {
                            throw new ValidationException($"Invalid Gender value: {testimonie.Victim.Gender}");
                        }

                        if (testimonie.Victim.Birthdate > DateTime.UtcNow)
                        {
                            throw new ValidationException($"Birthdate is not valid in future: {testimonie.Victim.Birthdate}");

                        }
                    }


                    if(!Enum.IsDefined(typeof(InjuryStatus), testimonie.InjuryStatus))
                    {
                        throw new ValidationException($"Invalid injury status value: {testimonie.InjuryStatus}");
                    }

                    if (testimonie.IssueDate > DateTime.UtcNow)
                    {
                        throw new ValidationException($"Issue Date is not valid in future: {testimonie.IssueDate}");

                    }
                }
            }

            if (!Enum.IsDefined(typeof(AreaType), incidentDTO.AreaType))
            {
                throw new ValidationException($"Invalid Area Type value: {incidentDTO.AreaType}");
            }

            if (!Enum.IsDefined(typeof(AreaClass), incidentDTO.AreaClass))
            {
                throw new ValidationException($"Invalid Area Class value: {incidentDTO.AreaClass}");
            }

            if(incidentDTO.DateOfOccurrence > DateTime.UtcNow)
            {
                throw new ValidationException($"Date Of Occurrence is not valid in future: {incidentDTO.DateOfOccurrence}");

            }

            var incident = _mapper.Map<Incident>(incidentDTO);
            

            await _incidentRepo.AddAsync(incident);

            if (intiaIncident != null)
                intiaIncident.Status = InitialIncidentReportStatus.PENDING;

            await _incidentRepo.SaveAsync();

            await _activityRepositoy.AddAsync(new Activity
            {
                Description = $"User with id '{incidentDTO.FieldResearcherId}' added new incident with id '{incident.Id}'",
                MadeById = incidentDTO.FieldResearcherId ?? "",
                Type = ActivityType.Add,

            });

            await _activityRepositoy.SaveAsync();

            var fullLoadedIncident = await _incidentRepo.GetFullByIdAsync(incident.Id);

            return _mapper.Map<ReturnFullIncidentDTO>(fullLoadedIncident);
        }

        public async Task<ReturnUpdatedIncidentDTO> UpdateIncident(UpdateIncidentDTO updateIncidentDTO, int IncidentId, string userId)
        {
            var incident = await _incidentRepo.GetWithTestimoniesOnlyById(IncidentId);

            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{IncidentId}' not found");

            if (incident.PreventModification)
                throw new ConflictException($"Incident with id '{IncidentId}' cannot be updated, " +
                    $"You need permission from the manager");

            if (incident.LegalTeamMemberId == null)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' is not assigned " +
                    $"to you");
            }else
            {
                if(incident.LegalTeamMemberId != userId)
                {
                    throw new ForbiddenException($"You are not allowed to update this incident");
                }
            }

            if (incident.DocumentationConsent)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' has a documentation consent" +
                    $", you can't update it");
            }

            if (incident.PublicationConsent)
            {
                throw new ConflictException($"you can't Modify this incident with id '{IncidentId}', it have been already published");
            }

            incident.DetailedDescription = updateIncidentDTO.DetailedDescription ?? incident.DetailedDescription;
            incident.AIClassification = updateIncidentDTO.AIClassification ?? incident.AIClassification;
            incident.PerpetratorDescription = updateIncidentDTO.PerpetratorDescription ?? incident.PerpetratorDescription;

            if(updateIncidentDTO.Testimonies != null)
            {
                foreach (var testimonyDTO in updateIncidentDTO.Testimonies)
                {
                    var testimony = incident.PersonalVictimTestimonies.FirstOrDefault(t => t.Id == testimonyDTO.Id);
                    if (testimony == null)
                        throw new DataNotFoundException($"Testimony with id '{testimonyDTO.Id}' not found in incident with id '{IncidentId}'");
                    testimony.PersonalNarrative = testimonyDTO.PersonalNarrative ?? testimony.PersonalNarrative;
                }
            }
            incident.PreventModification = true;


            await _activityRepositoy.AddAsync(new Activity
            {
                Description = $"User with id '{userId}' updated incident with id '{incident.Id}'",
                MadeById = userId,
                Type = ActivityType.Update,

            });

            await _incidentRepo.SaveAsync();

            return _mapper.Map<ReturnUpdatedIncidentDTO>(incident);  
        }

        public async Task<PagedResultDTO<List<ReturnIncidentDTO>>> GetFieldResearcherIncidentsByPageAsync(
         PaginationDTO pageDTO, string userId, string? searchVictimNationalId, bool OrderByDateOfOccurence,
         bool? DocumentationConsent, bool? PublicationConsent)
        {
            var (incidents, totalItems) = await _incidentRepo.GetFieldResearcherIncidentsByPageAsync((pageDTO.Page - 1) * pageDTO.PageSize,
                pageDTO.PageSize, userId, searchVictimNationalId, OrderByDateOfOccurence,
                DocumentationConsent,PublicationConsent);

            var incidentDTOs = _mapper.Map<List<ReturnIncidentDTO>>(incidents);

            return new PagedResultDTO<List<ReturnIncidentDTO>>()
            {
                Data = incidentDTOs,
                Page = pageDTO.Page,
                PageSize = pageDTO.PageSize,
                TotalCount = totalItems
            };
        }

        public async Task<PagedResultDTO<List<ReturnIncidentDTO>>> GetAllIncidentsByPageAsync(PaginationDTO pageDTO, int? cityId
          , bool OrderByDateOfOccurence, bool? DocumentationConsent, bool? PublicationConsent, int? Sensitivity)
        {
            if(Sensitivity.HasValue && (Sensitivity < 1 || Sensitivity > 10))
            {
                throw new ValidationException("Validation Faild", new { Sensitivity = "Must be between 10 and 1"});
            }

            var (incidents, totalItems) = await _incidentRepo.GetAllIncidentsByPageAsync((pageDTO.Page - 1) * pageDTO.PageSize,
                pageDTO.PageSize, cityId, OrderByDateOfOccurence, DocumentationConsent, PublicationConsent, Sensitivity);

            var incidentDTOs = _mapper.Map<List<ReturnIncidentDTO>>(incidents);

            return new PagedResultDTO<List<ReturnIncidentDTO>>()
            {
                Data = incidentDTOs,
                Page = pageDTO.Page,
                PageSize = pageDTO.PageSize,
                TotalCount = totalItems
            };
        }

        public async Task<List<ReturnEvidenceDTO>> AddRangeOfRelatedEvidences(List<AddEvidenceDTO> evidenceDTOs, int incidentId)
        {
            if (evidenceDTOs == null || evidenceDTOs.Count == 0)
                throw new ValidationException("No evidences provided");


            var incident = await _incidentRepo.GetByIdAsync(incidentId);
            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{incidentId}' not found");


            var uploadTasks = evidenceDTOs.Select(async item =>
            {
                if (item.File == null || item.File.Length == 0)
                    throw new ValidationException("Invalid file provided");

                if (!Enum.IsDefined(typeof(EvidenceType), item.Type))
                    throw new ValidationException($"Invalid evidence type: {item.Type}");

                if(FileTypeValidator.IsMatchingType(item.File.ContentType, item.Type) == false)
                    throw new ValidationException($"File type {item.File.ContentType} does not match evidence type {item.Type}");

                if(item.CaptureDate > DateTime.UtcNow)
                    throw new ValidationException($"Capture date is not valid in future: {item.CaptureDate}");

                var uploadResult = await _cloudinaryService.UploadAsync(item.File, item.Type);

                return new Evidence
                {
                    CloudinaryUrl = uploadResult.Url,
                    CloudinaryPublicId = uploadResult.PublicId,
                    Type = item.Type,
                    CaptureDate = DateTime.SpecifyKind(item.CaptureDate, DateTimeKind.Utc),
                    Description = item.Description,
                    IncidentId = incidentId
                };
            });

            var evidenceEntities = (await Task.WhenAll(uploadTasks)).ToList();

            await _incidentRepo.AddRangeOfEvidencesAsync(evidenceEntities);

            await _incidentRepo.SaveAsync();

            return _mapper.Map<List<ReturnEvidenceDTO>>(evidenceEntities);  
        }

        public async Task<List<ReturnEvidenceDTO>> GetEvidencesByIncidentIdAsync(int incidentId)
        {
            var incident = await _incidentRepo.GetByIdAsync(incidentId);
            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{incidentId}' not found");

            var evidences = await _incidentRepo.GetEvidencesByIncidentIdAsync(incidentId);
            
            return _mapper.Map<List<ReturnEvidenceDTO>>(evidences);
        }

        public async Task<List<ReturnVictimTestimonieDTO>> GetTestimoniesAndTheirVictimsByIncidentIdAsync(int incidentId)
        {
            var incident = await _incidentRepo.GetByIdAsync(incidentId);

            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{incidentId}' not found");

            var testimonies = await _incidentRepo.GetTestimoniesAndTheirVictimsByIncidentIdAsync(incidentId);
            return _mapper.Map<List<ReturnVictimTestimonieDTO>>(testimonies);
        }

        public async Task<ReturnGiveDocumentationConsentDTO> GiveDocumentationConsentAsync(int IncidentId, string userId)
        {
            var incident = await _incidentRepo.GetByIdAsync(IncidentId);

            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{IncidentId}' not found");

            if (incident.DocumentationConsent)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' has a documentation consent" +
                    $"already");
            }

            if (incident.LegalTeamMemberId == null)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' is not assigned " +
                    $"to you");
            }
            else
            {
                if (incident.LegalTeamMemberId != userId)
                {
                    throw new ForbiddenException($"You are not allowed to give documentation consent to this incident");
                }
            }

            incident.DocumentationConsent = true;
            incident.PreventModification = true;

            if (incident.InitialIncidentReportId != null)
            {
                var intitialReport = await _initialIncidentReportRepo.GetByIdAsync(incident.InitialIncidentReportId.Value);

                if (intitialReport == null)
                    throw new ConflictException($"This incident has intial report id '{incident.InitialIncidentReportId.Value}'" +
                        $", but there is no intial report entity found with this Id");

                intitialReport.Status = InitialIncidentReportStatus.CLOSED;
            }

            await _activityRepositoy.AddAsync(new Activity
            {
                Description = $"User with id '{userId}' give incident with id '{incident.Id} a documentation consent'",
                MadeById = userId,
                Type = ActivityType.Update,

            });

            await _incidentRepo.SaveAsync();

            return _mapper.Map<ReturnGiveDocumentationConsentDTO>(incident);
        }


        public async Task RequestModificationAsync(int IncidentId, string userId)
        {
            var incident = await _incidentRepo.GetByIdAsync(IncidentId);
            
            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{IncidentId}' not found");

            if (!incident.PreventModification)
            {
                throw new ConflictException($"Modification request cannot be sent for incident with id '{IncidentId}' " +
                    $"because it is not locked for modification");
            }

            if (incident.PublicationConsent)
            {
                throw new ConflictException($"you can't Modify this incident with id '{IncidentId}', it has been already published");
            }

            if (incident.LegalTeamMemberId == null)
            {
                throw new ConflictException($"Incident with id '{IncidentId}' is not assigned " +
                    $"to you, you can't modify it");
            }
            else
            {
                if (incident.LegalTeamMemberId != userId)
                {
                    throw new ForbiddenException($"You are not allowed to request modification for this incident");
                }
            }

            await _activityRepositoy.AddAsync(new Activity
            {
                Description = $"Legal team member with id '{userId}' requested modification for incident with id '{incident.Id}'",
                MadeById = userId,
                Type = ActivityType.RequestChange
            });

            await _activityRepositoy.SaveAsync();
        }

        public async Task<ReturnGiveDocumentationConsentDTO> AllowModificationAsync(int IncidentId, string userId)
        {
            var incident = await _incidentRepo.GetByIdAsync(IncidentId);

            if (incident == null)
                throw new DataNotFoundException($"Incident with id '{IncidentId}' not found");

            if (incident.PreventModification == false)
            {
                throw new ConflictException($"Modification on this incident with id '{IncidentId}' was already allowed");
            }

            if(incident.PublicationConsent)
            {
                throw new ConflictException($"you can't Modify this incident with id '{IncidentId}', it has been already published");
            }

            incident.PreventModification = false;
            incident.DocumentationConsent = false;

            if (incident.InitialIncidentReportId != null)
            {
                var intitialReport = await _initialIncidentReportRepo.GetByIdAsync(incident.InitialIncidentReportId.Value);

                if (intitialReport == null)
                    throw new ConflictException($"This incident has intial report id '{incident.InitialIncidentReportId.Value}'" +
                        $", but there is no intial report entity found with this Id");

                intitialReport.Status = InitialIncidentReportStatus.PENDING;
            }

            await _activityRepositoy.AddAsync(new Activity
            {
                Description = $"Manager with id '{userId}' allow modification on this incident with id '{incident.Id}'",
                MadeById = userId,
                Type = ActivityType.Update,

            });

            await _incidentRepo.SaveAsync();

            return _mapper.Map<ReturnGiveDocumentationConsentDTO>(incident);
        }

    }
}
