using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace BuildingBlocks.Extensions;

public static class ResultExtensions
{
    public static IResult ToResponse<TValue, TErrorList>(this Result<TValue, TErrorList> result)
        where TErrorList : List<Error>
    {
        return result.Error[0].Type switch
        {
            ErrorType.VALIDATION => Results.BadRequest(new ProblemDetails()),
            ErrorType.CONFLICT => Results.Conflict(result.Error[0]),
            ErrorType.INTERNAL => Results.InternalServerError(result.Error[0]),
            ErrorType.NOT_FOUND => Results.NotFound(result.Error[0]),
            _ => Results.BadRequest(result.Error[0])
        };
    }
}