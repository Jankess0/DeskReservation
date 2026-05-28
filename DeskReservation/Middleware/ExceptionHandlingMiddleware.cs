using System.Net;
using System.Text.Json;

namespace DeskReservation.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, $"Something went wrong: {ex.Message}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,       
            UnauthorizedAccessException => HttpStatusCode.Forbidden, 
            ArgumentException => HttpStatusCode.BadRequest,         
            _ => HttpStatusCode.InternalServerError               
        };

        context.Response.StatusCode = (int)statusCode;
        
        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,

            Detailed = context.Response.StatusCode == 500 ? "Server error." : exception.Message
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsJsonAsync(response, jsonOptions);
    }
}