using System;
using BL.Helper;
using BL.MappingProfiles;
using BL.Models;
using BL.Queue;
using BL.Queue.Interfaces;
using BL.Services;
using BL.Services.Interfaces;
using BL.Workers;
using CloudinaryDotNet;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace BL.Extensions
{
    public static class BusinessLogicRegistration
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register AutoMapper Service
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MainMappingProfile>();
            });

            // Register Cloudinary Service
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            
            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;

                var account = new Account(
                    config.CloudName,
                    config.ApiKey,
                    config.ApiSecret
                );

                return new Cloudinary(account);
            });
            

            // Register The Business Services
            services.AddScoped<IInitialIncidentReportService, InitialIncidentReportService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<IVictimService, VictimService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<ILegalNoteService, LegalNoteService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<INewsItemService, NewsItemService>();
            services.AddScoped<IStatsService, StatsService>();


            // Register Services used By Classifier
            services.AddSingleton<IIncidentClassificationQueue, IncidentClassificationQueue>();

            services.AddHttpClient<IEmbeddingService, GeminiEmbeddingService>();
            services.AddHttpClient<IClassificationService<IncidentClassificationInput, MatchedArticle, ClassificationResult>
                , GroqClassificationService>();
            services.AddScoped<IVectorStoreService<LegalLawItem, MatchedArticle>, QdrantVectorStoreService>();
            services.AddScoped<ISeeder<LegalLawItem>, RomeStatuteSeeder>();

            services.AddHostedService<ClassificationWorker>();

           


            services.AddScoped<DTOBuilder>();

            return services;
        }
    }
}
