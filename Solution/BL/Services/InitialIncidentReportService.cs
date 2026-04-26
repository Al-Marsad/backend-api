using AutoMapper;
using BL.DTO.General;
using BL.DTO.InitialIncidentReport;
using BL.Helper;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class InitialIncidentReportService : IInitialIncidentReportService
    {
        private readonly IInitialIncidentReportRepository _initialReportRepo;
        private readonly IMapper _mapper;
        public InitialIncidentReportService(IInitialIncidentReportRepository initialReportRepo,
            IMapper mapper)
        {
            this._initialReportRepo = initialReportRepo;
            this._mapper = mapper;
        }
        public async Task<ReturnInitialIncidentReportDTO> AddAsync(AddInitialIncidentReportDTO reportDto)
        {
            var reportEntity = _mapper.Map<InitialIncidentReport>(reportDto);

            if (reportEntity == null) { 
                throw new ArgumentNullException("There is something wrong in the sent fields");
            }

            await _initialReportRepo.AddAsync(reportEntity);

            await _initialReportRepo.SaveAsync();

            return  _mapper.Map<ReturnInitialIncidentReportDTO>(reportEntity);
        }

        public async Task<ReturnDetailedInitialIncidentReportDTO> GetByIdAsync(int id, string userId)
        {
            var report = await this._initialReportRepo.GetByIdAsync(id);

            if (report == null)
            {
                throw new DataNotFoundException("There is no intial report with this id");
            }

            if(report.CitizenReporterId != userId)
            {
                throw new ForbiddenException("The report belongs to a different citizen");
            }

            return _mapper.Map<ReturnDetailedInitialIncidentReportDTO>(report);
        }

        public async Task<PagedResultDTO<List<ReturnDetailedInitialIncidentReportDTO>>> GetByPageAsync(
            GetByPageInitialIncidentReportDTO reportDTO,
            CurrentUser user
           )
        {

            if (reportDTO.Status.HasValue && !Enum.IsDefined(typeof(InitialIncidentReportStatus), reportDTO.Status.Value))
                throw new ValidationException("Validation failed", new { Status = "Value is invalid" });

            string? userId = null;

            if (user.Role == RolesSelector.FieldResearcher)
            {
                if (!reportDTO.CityId.HasValue)
                    reportDTO.CityId = Convert.ToInt32(user.CityId);
            }
            else
            {
                userId = user.UserId;

                if (userId == null)
                {
                    throw new UnauthorizedException("JWT missing or expired");
                }
            }

            var (reports, totalItems) = await _initialReportRepo.GetPageAsync(
                (reportDTO.Page - 1) * reportDTO.PageSize,
                reportDTO.PageSize,
                userId,
                reportDTO.Status,
                reportDTO.CityId);

            var reportDTOs = _mapper.Map<List<ReturnDetailedInitialIncidentReportDTO>>(reports);

            return new PagedResultDTO<List<ReturnDetailedInitialIncidentReportDTO>>()
            {
                Data = reportDTOs,
                Page = reportDTO.Page,
                PageSize = reportDTO.PageSize,
                TotalCount = totalItems
            };
        }
        public async Task<ReturnInitialIncidentReportDTO> AssignToFieldResearcher(AssignToFieldResearcherDTO data)
        {
            var report = await _initialReportRepo.GetByIdAsync(data.ReportId);
            if (report == null)
            {
                throw new DataNotFoundException("There is no initial report with this id");
            }
           
            if(report.FieldResearcherId != null)
            {
                throw new ConflictException("Initial report was already assigned to a field researcher");

            }

            report.FieldResearcherId = data.FieldResearcherId;
            report.Status = InitialIncidentReportStatus.ASSIGNED;

            await _initialReportRepo.SaveAsync();

            return _mapper.Map<ReturnInitialIncidentReportDTO>(report);
        }

        public async Task<ReturnInitialIncidentReportDTO> UnassignToFieldResearcher(AssignToFieldResearcherDTO data)
        {
            var report = await _initialReportRepo.GetByIdAsync(data.ReportId);
            if (report == null)
            {
                throw new DataNotFoundException("There is no initial report with this id");
            }

            if (report.Status != InitialIncidentReportStatus.ASSIGNED)
            {
                throw new ConflictException("initial report can't be unassigned due to its status");
            }

            if (report.FieldResearcherId != data.FieldResearcherId)
            {
                throw new ForbiddenException($"Initial report was not assigned to current field researcher");

            }

            report.FieldResearcherId = null;
            report.Status = InitialIncidentReportStatus.UNASSIGNED;

            await _initialReportRepo.SaveAsync();

            return _mapper.Map<ReturnInitialIncidentReportDTO>(report);
        }

        public List<StatusValuesDTO> GetStatusValues()
        {
            return _mapper.Map<List<StatusValuesDTO>>(Enum.GetValues<InitialIncidentReportStatus>().ToList());
        }

        public async Task<PagedResultDTO<List<ReturnDetailedInitialIncidentReportDTO>>> GetMyAssignedReportsAsync(
            string userId,
            PaginationDTO paginationDTO, 
            string? search = null)
        {
            var Skip = (paginationDTO.Page - 1) * paginationDTO.PageSize;
            var Take = paginationDTO.PageSize;

            var (reports, totalItems) = await _initialReportRepo.GetAssignedReportsAsync(userId, Skip, Take, search);

            var reportDTOs = _mapper.Map<List<ReturnDetailedInitialIncidentReportDTO>>(reports);

            return new PagedResultDTO<List<ReturnDetailedInitialIncidentReportDTO>>
            {
                Data = reportDTOs,
                Page = paginationDTO.Page,
                PageSize = paginationDTO.PageSize,
                TotalCount = totalItems
            };
        }
    }
}