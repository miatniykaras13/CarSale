using BuildingBlocks.Errors;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Extensions;

public static class ErrorExtensions
{
    public static IResult ToResponse(this Error error)
    {
        return error.Type switch
        {
            ErrorType.VALIDATION => Results.BadRequest(error),
            ErrorType.CONFLICT or ErrorType.DOMAIN => Results.Conflict(error),
            ErrorType.INTERNAL => Results.InternalServerError(error),
            ErrorType.NOT_FOUND => Results.NotFound(error),
            ErrorType.FORBIDDEN => Results.Json(error, statusCode: StatusCodes.Status403Forbidden),
            _ => Results.BadRequest(error)
        };
    }

    public static IResult ToResponse(this IList<Error> errors)
    {
        return errors[0].Type switch
        {
            ErrorType.VALIDATION => Results.BadRequest(errors),
            ErrorType.CONFLICT or ErrorType.DOMAIN => Results.Conflict(errors),
            ErrorType.INTERNAL => Results.InternalServerError(errors),
            ErrorType.NOT_FOUND => Results.NotFound(errors),
            ErrorType.FORBIDDEN => Results.Json(errors, statusCode: StatusCodes.Status403Forbidden),
            _ => Results.BadRequest(errors)
        };
    }
}