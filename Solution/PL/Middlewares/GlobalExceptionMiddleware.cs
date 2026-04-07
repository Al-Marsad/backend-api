using DAL.Exceptions;

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
            int statusCode = 500;
            string code = "INTERNAL_ERROR";
            object? fields = null;

            if (ex is BusinessException bizEx)
            {
                statusCode = bizEx.StatusCode;
                code = bizEx.Code;
                fields = bizEx.Fields;
            }

            var response = new
            {
                Success = false,
                Error = new
                {
                    Code = code,
                    Message = ex.Message,
                    Fields = fields
                }
            };


                context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(response);
        }


    }
}
