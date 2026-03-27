using AutoMapper;
using BL.DTO.InitialIncidentReport;
using DAL.Entities;


namespace BL.MappingProfiles
{
    public class MainMappingProfile : Profile
    {
        public MainMappingProfile() {
            // Initial Incident Report Profile
            CreateMap<AddInitialIncidentReportDTO, InitialIncidentReport>();
            CreateMap<InitialIncidentReport, ReturnInitialIncidentReportDTO>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
