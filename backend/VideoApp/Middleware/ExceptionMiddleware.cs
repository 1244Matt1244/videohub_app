using System.Net;
using System.Text.Json;

namespace VideoApp.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (UnauthorizedAccessException ex) { await WriteError(context, HttpStatusCode.Unauthorized, ex.Message); }
        catch (KeyNotFoundException ex) { await WriteError(context, HttpStatusCode.NotFound, ex.Message); }
        catch (ArgumentException ex) { await WriteError(context, HttpStatusCode.BadRequest, ex.Message); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteError(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteError(HttpContext ctx, HttpStatusCode code, string message)
    {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)code;
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
    }
}
