using AutoMapper;
using BL.DTO.InitialIncidentReport;
using BL.DTO.User;
using DAL.Entities;


namespace BL.MappingProfiles
{
    public class MainMappingProfile : Profile
    {
        public MainMappingProfile() {
            // Initial Incident Report Profile
            CreateMap<AddInitialIncidentReportDTO, InitialIncidentReport>();
            CreateMap<InitialIncidentReport, ReturnInitialIncidentReportDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // User Profile
            CreateMap<AddUserDTO, AppUser>()
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Birthdate, DateTimeKind.Utc)));
            CreateMap<AppUser, ReturnRegisteredUserDTO>();
            CreateMap<AppUser, ReturnLoginUserDTO>();

        }
    }
}
