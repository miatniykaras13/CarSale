using BuildingBlocks.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Extensions;

public static class ErrorExtensions
{
    public static IResult ToProblemDetails(this Error error, HttpContext context)
    {
        (string Title, string? Detail, string Instance) details =
            ("CustomError", error.Message, context.Request.Path);

        var statusCode = error.Type switch
        {
            ErrorType.VALIDATION => StatusCodes.Status400BadRequest,
            ErrorType.CONFLICT or ErrorType.DOMAIN => StatusCodes.Status409Conflict,
            ErrorType.INTERNAL => StatusCodes.Status500InternalServerError,
            ErrorType.NOT_FOUND => StatusCodes.Status404NotFound,
            ErrorType.FORBIDDEN => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails()
        {
            Title = details.Title, Detail = details.Detail, Instance = details.Instance, Status = statusCode,
        };

        problemDetails.Extensions.Add("errorCode", error.Code);
        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        return Results.Problem(problemDetails);
    }

    public static IResult ToProblemDetails(this IList<Error> errors, HttpContext context) => errors[0].ToProblemDetails(context);
}