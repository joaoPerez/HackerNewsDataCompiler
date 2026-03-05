using HackerNewsDataCompiler.API.Domain.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace HackerNewsDataCompiler.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception of type {ExceptionType} occurred.", exception.GetType().Name);

            var (statusCode, message) = exception switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found."),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized."),
                ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.")
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                ResponseModel.Failure(statusCode, message),
                cancellationToken);

            return true;
        }
    }
}
