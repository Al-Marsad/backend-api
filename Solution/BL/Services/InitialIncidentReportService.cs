using AutoMapper;
using BL.DTO.InitialIncidentReport;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Exceptions;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<List<ReturnDetailedInitialIncidentReportDTO>> GetByPageAsync(int page, int pageSize, string userId)
        {
            if(page < 1)
            {
                throw new ValidationException("Validation failed", new {Page = "Can't be less than 1" });
            }

            if(pageSize < 0 || pageSize > 50)
            {
                throw new ValidationException("Validation failed", new {PageSize = "Can't be less than 0 or greater than 50"});

            }

            var reports = await _initialReportRepo.GetPageAsync((page - 1) * pageSize, pageSize, userId);
            var returnReports = _mapper.Map<List<ReturnDetailedInitialIncidentReportDTO>>(reports);

            return returnReports;
        }

        public async Task<List<ReturnDetailedInitialIncidentReportDTO>> GetByPageAsync(int page, int pageSize, string userId, InitialIncidentReportStatus status)
        {
            if (page < 1)
            {
                throw new ValidationException("Validation failed", new { Page = "Can't be less than 1" });
            }

            if (pageSize < 0 || pageSize > 50)
            {
                throw new ValidationException("Validation failed", new { PageSize = "Can't be less than 0 or greater than 50" });

            }

            if (!Enum.IsDefined(typeof(InitialIncidentReportStatus), status)) {
                throw new ValidationException("Validation failed", new {Status = "Value is invalid"});
            }

            var reports = await _initialReportRepo.GetPageAsync((page - 1) * pageSize, pageSize, userId, status);
            var returnReports = _mapper.Map<List<ReturnDetailedInitialIncidentReportDTO>>(reports);

            return returnReports;
        }
        public List<StatusValuesDTO> GetStatusValues()
        {
            return _mapper.Map<List<StatusValuesDTO>>(Enum.GetValues<InitialIncidentReportStatus>().ToList());
        }
    }
}