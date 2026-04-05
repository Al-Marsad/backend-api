
using System.Text;
using BL.Extensions;
using DAL.DBContext;
using DAL.Entities;
using DAL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PL.Middlewares;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Add DAL Services
            builder.Services.AddDataAccess(builder.Configuration);

            // Add BL Services
            builder.Services.AddBusinessLogic();

            // Customize Response Of Automatic Validation
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var fields = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.First().ErrorMessage
                        );

                    var result = new ObjectResult(new
                    {
                        Success = false,
                        Error = new
                        {
                            Code = "VALIDATION_ERROR",
                            Message = "Validation failed",
                            Fields = fields
                        }
                    })
                    {
                        StatusCode = 422
                    };

                    return result;
                };
            });

            // Spacify The Authentication Type To Jwt Authentication
            var audiences = builder.Configuration.GetSection("JWT:Audiences").Get<string[]>()
                 ?? Array.Empty<string>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudiences = audiences,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Token failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    }
                    ,
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("{\"Success\": false, \"Error\": {\"Code\": \"UNAUTHORIZED\", \"Message\": \"JWT missing or expired\"}}");
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("{\"Success\": false, \"Error\": {\"Code\": \"FORBIDDEN\", \"Message\": \"You do not have the required Role to access this resource.\"}}");
                    }
                };
            });
            builder.Services.AddAuthorization();


            // Create A Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyDevPolicy", policy =>
                {
                    policy.WithOrigins(audiences)
                          .AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("MyDevPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
