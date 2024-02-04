using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Shared.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try {
            await _next(context);
        } catch (Exception error) {
            var response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };
            _logger.LogError("Unhandled error {err}", error.Message);

            await response.WriteAsJsonAsync(new { message = error.Message });
        }
    }
}