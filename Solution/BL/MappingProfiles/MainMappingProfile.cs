using AutoMapper;
using BL.DTO.City;
using BL.DTO.InitialIncidentReport;
using BL.DTO.User;
using DAL.Entities;
using DAL.Enums;


namespace BL.MappingProfiles
{
    public class MainMappingProfile : Profile
    {
        public MainMappingProfile() {
            // Initial Incident Report Profile
            CreateMap<AddInitialIncidentReportDTO, InitialIncidentReport>();
            CreateMap<InitialIncidentReport, ReturnInitialIncidentReportDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<InitialIncidentReport, ReturnDetailedInitialIncidentReportDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<InitialIncidentReportStatus, StatusValuesDTO>()
                .ForMember(dest => dest.StatuName, opt => opt.MapFrom(src => src.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src));

            // User Profile
            CreateMap<AddUserDTO, AppUser>()
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Birthdate, DateTimeKind.Utc)));
            CreateMap<AppUser, ReturnRegisteredUserDTO>();
            CreateMap<AppUser, ReturnLoginUserDTO>();
            CreateMap<AppUser, GetUserPorfileDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                src.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault()));
            CreateMap<AccountStatus, StatusValuesDTO>()
                .ForMember(dest => dest.StatuName, opt => opt.MapFrom(src => src.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src));

            // City Profile
            CreateMap<AddCityDTO, City>();
            CreateMap<City, ReturnCityDTO>();


        }
    }
}
