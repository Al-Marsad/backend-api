using AutoMapper;
using BL.DTO.InitialIncidentReport;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Exceptions;
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


    }
}