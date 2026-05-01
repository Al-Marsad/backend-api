using DAL.Entities;
using DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace PL.Middlewares
{
    public class ActiveUserMiddleware
    {
        private readonly RequestDelegate _next;

        public ActiveUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<AppUser> userManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var user = await userManager.GetUserAsync(context.User);

                if (user != null && user.AccountStatus == AccountStatus.Inactive)
                {

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        Success = false,
                        Error = new
                        {
                            Code = "FORBIDDEN",
                            Message = "Your account is inactive. Please contact support.",
                            Fields = (object?)null
                        }
                    };

                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }
            }

            await _next(context);
        }
    }
}