using AutoMapper;
using BL.DTO.InitialIncidentReport;
using BL.Services.Interfaces;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace BL.Services
{
    public class InitialIncidentReportService : IInitialIncidentReportService
    {
        private readonly IInitialIncidentReportRepository _initialReportService;
        private readonly IMapper _mapper;
        public InitialIncidentReportService(IInitialIncidentReportRepository initialReportService,
            IMapper mapper)
        {
            this._initialReportService = initialReportService;
            this._mapper = mapper;
        }
        public async Task<ReturnInitialIncidentReportDTO> AddAsync(AddInitialIncidentReportDTO reportDto)
        {
            var reportEntity = _mapper.Map<InitialIncidentReport>(reportDto);

            if (reportEntity == null) { 
                throw new ArgumentNullException("There is something wrong in the sent fields");
            }

            await _initialReportService.AddAsync(reportEntity);

            await _initialReportService.SaveAsync();

            return  _mapper.Map<ReturnInitialIncidentReportDTO>(reportEntity);
        }

    }
}