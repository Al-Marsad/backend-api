using DAL.DBContext;
using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Extensions
{
    public static class DataAccessRegistration
    {
        public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            // Add DbContext Options
            services.AddDbContext<AlMarsadDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());

            // Add Identity Service 
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AlMarsadDbContext>()
            .AddDefaultTokenProviders();
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(1);
            });

            // Add Application Repositories
            services.AddScoped<IInitialIncidentReportRepository, InitialIncidentReportRepository>();
            services.AddScoped<ICityRepository, CityRepository>();


            return services;
        }
    }
}
