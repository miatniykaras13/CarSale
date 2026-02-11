using BuildingBlocks.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Extensions;

public static class ProblemDetailsExtensions
{
    public static Error ToError(this ProblemDetails details)
    {
        var errorType = details.Status switch
        {
            StatusCodes.Status400BadRequest => ErrorType.VALIDATION,
            StatusCodes.Status409Conflict => ErrorType.CONFLICT,
            StatusCodes.Status500InternalServerError => ErrorType.INTERNAL,
            StatusCodes.Status404NotFound => ErrorType.NOT_FOUND,
            StatusCodes.Status403Forbidden => ErrorType.FORBIDDEN,
            _ => ErrorType.INTERNAL
        };

        var code = "unknown";

        if (details.Title?.Equals("CustomError", StringComparison.OrdinalIgnoreCase) == true)
        {
            code = details.Extensions["errorCode"]?.ToString() ?? code;
        }

        var message = details.Detail;

        return Error.Custom(code, message, errorType);
    }
}