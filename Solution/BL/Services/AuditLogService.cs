using BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace BL.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ILogger<AuditLogService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogService(
            ILogger<AuditLogService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public void LogAction(string description)
        {
            var context = _httpContextAccessor.HttpContext;

            var userId = context?.User?.Identity?.Name ?? "Anonymous";

            using (LogContext.PushProperty("LogType", "Manager"))
            using (LogContext.PushProperty("UserId", userId))
            {
                _logger.LogInformation(description);
            }
        }
    }
}
