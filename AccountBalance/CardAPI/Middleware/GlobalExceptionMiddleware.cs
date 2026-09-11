using System.Net;
using System.Text.Json;

namespace CardAPI.Middleware
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
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                FluentValidation.ValidationException => (HttpStatusCode.BadRequest, exception.Message),
                Microsoft.Data.SqlClient.SqlException sqlEx when sqlEx.Number >= 50000 && sqlEx.Number < 51000
                    => (HttpStatusCode.BadRequest, sqlEx.Message), // errores de negocio lanzados con THROW en tus SPs
                Microsoft.Data.SqlClient.SqlException
                    => (HttpStatusCode.InternalServerError, "Error de base de datos."),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "No autorizado."),
                _ => (HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = (int)statusCode,
                message,
                traceId = context.TraceIdentifier
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
