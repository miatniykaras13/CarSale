using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handlers;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error occured: {ExceptionMessage}. At the time {Time}", exception.Message, DateTime.UtcNow);

        (string Details, string Title, int StatusCode) details = exception switch
        {
            _ => (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails()
        {
            Title = details.Title,
            Detail = details.Details,
            Status = details.StatusCode,
            Instance = context.Request.Path,
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}