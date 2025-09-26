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
            ErrorType.VALIDATION => Results.BadRequest(string.Join(',', result.Error.Select(e => e.Message))),
            ErrorType.CONFLICT => Results.Conflict(result.Error[0].Code),
            ErrorType.INTERNAL => Results.InternalServerError(result.Error[0].Code),
            ErrorType.NOT_FOUND => Results.NotFound(result.Error[0].Code),
            _ => Results.BadRequest(result.Error[0].Code)
        };
    }
}