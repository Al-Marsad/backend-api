using DAL.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PL.Helper;

namespace PL.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode = StatusCodes.Status500InternalServerError;
            string code = "INTERNAL_ERROR";
            object? fields = null;
            string message = "An unexpected error occurred";

            if (ex is BusinessException bizEx)
            {
                statusCode = bizEx.StatusCode;
                code = bizEx.Code;
                fields = bizEx.Fields;
                message = bizEx.Message;
            }

            else if (ex is DbUpdateException dbEx &&
                     dbEx.InnerException is PostgresException pgEx &&
                     pgEx.SqlState == "23505")
            {
                statusCode = StatusCodes.Status409Conflict;
                code = "DUPLICATE_RESOURCE";
                message = "Duplicate resource";

                fields = ConstraintMapper.MapPostgresConstraint(pgEx.ConstraintName);
            }

            var response = new
            {
                Success = false,
                Error = new
                {
                    Code = code,
                    Message = message,
                    Fields = fields
                }
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
