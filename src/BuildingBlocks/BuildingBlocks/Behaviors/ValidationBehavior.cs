using System.Reflection;
using BuildingBlocks.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var invalidResults = validationResults.Where(result => !result.IsValid).ToList();

        if (invalidResults.Count == 0)
            return await next(cancellationToken);

        var validationErrors = invalidResults.SelectMany(result => result.Errors).ToList();

        var errors = validationErrors.ToErrors(GetObjectPrefix(typeof(TRequest)));

        var responseType = typeof(TResponse);

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<,>))
        {
            var genericArguments = responseType.GetGenericArguments();
            var valueType = genericArguments[0];
            var errorType = genericArguments[1];

            var failureMethodGeneric = typeof(Result)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m is { Name: "Failure", IsGenericMethodDefinition: true } &&
                            m.GetGenericArguments().Length == 2)
                .Select(m => new { Method = m, Params = m.GetParameters() })
                .FirstOrDefault(x => x.Params.Length == 1)?.Method;

            if (failureMethodGeneric == null)
                throw new InvalidOperationException("Cannot find Result.Failure generic method.");

            var constructedFailure = failureMethodGeneric.MakeGenericMethod(valueType, errorType)
                .Invoke(null, [errors]);

            return (TResponse)constructedFailure!;
        }

        if (responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == typeof(UnitResult<>))
        {
            var genericArguments = responseType.GetGenericArguments();
            var errorType = genericArguments[0];

            var failureMethodGeneric = typeof(UnitResult)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m is { Name: "Failure", IsGenericMethodDefinition: true } &&
                            m.GetGenericArguments().Length == 1)
                .Select(m => new { Method = m, Params = m.GetParameters() })
                .FirstOrDefault(x => x.Params.Length == 1)?.Method;

            if (failureMethodGeneric == null)
                throw new InvalidOperationException("Cannot find UnitResult.Failure generic method.");

            var constructedFailure = failureMethodGeneric.MakeGenericMethod(errorType)
                .Invoke(null, [errors]);

            return (TResponse)constructedFailure!;
        }

        throw new ValidationException("Validation failed");
    }

    private string GetObjectPrefix(Type requestType)
    {
        string name = requestType.Name.ToLowerInvariant();
        if (name.Contains("brand")) return "brand";
        if (name.Contains("model")) return "model";
        if (name.Contains("generation")) return "generation";
        if (name.Contains("engine")) return "engine";
        if (name.Contains("car")) return "car";
        if (name.Contains("ad")) return "ad";
        if (name.Contains("caroption")) return "car_option";
        if (name.Contains("bodytype")) return "body_type";
        if (name.Contains("transmissiontype")) return "transmission_type";
        if (name.Contains("autodrivetype")) return "drive_type";
        if (name.Contains("fueltype")) return "fuel_type";
        return "entity";
    }
}