using BuildingBlocks.Errors;
using Microsoft.IdentityModel.Tokens;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace BuildingBlocks.Extensions;

public static class FluentValidationExtensions
{
    public static Error ToError(this ValidationFailure failure, string obj) =>
        Error.Validation($"{obj}.{failure.PropertyName.ToLowerInvariant()}", failure.ErrorMessage);

    public static List<Error> ToErrors(this List<ValidationFailure> failure, string obj) =>
        failure.Select(x => x.ToError(obj)).ToList();
}