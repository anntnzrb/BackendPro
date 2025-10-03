using System.Net;
using System.Text.Json;
using BackendPro.Core.Exceptions;

namespace BackendPro.Web.Middleware;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode;
        string message;
        IReadOnlyDictionary<string, string[]>? errors = null;

        switch (exception)
        {
            case EntityNotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound;
                message = notFoundEx.Message;
                break;
            case BusinessValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest;
                message = validationEx.Message;
                errors = validationEx.Errors;
                break;
            case DuplicateEntityException duplicateEx:
                statusCode = HttpStatusCode.Conflict;
                message = duplicateEx.Message;
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An internal error occurred. Please try again later.";
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            errors,
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, _jsonOptions)
        ).ConfigureAwait(false);
    }
}
