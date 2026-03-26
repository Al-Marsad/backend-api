
using Microsoft.Extensions.DependencyInjection;

namespace BL.Extensions
{
    public static class BusinessLogicRegistration
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            return services;
        }
    }
}
