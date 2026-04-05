using AutoMapper;
using BL.MappingProfiles;
using BL.Services;
using BL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BL.Extensions
{
    public static class BusinessLogicRegistration
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            // Register AutoMapper Service
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MainMappingProfile>();
            });

            // Register The Business Services
            services.AddScoped<IInitialIncidentReportService, InitialIncidentReportService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
