using System.Diagnostics;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[START] Handle request = {Request}, Response = {Response}, Request data = {ResponseData}",
            typeof(TRequest).Name,
            ToFriendlyTypeName(typeof(TResponse)),
            request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next(cancellationToken);

        timer.Stop();

        var timeTaken = timer.Elapsed;

        if (timeTaken.Seconds > 3)
        {
            logger.LogWarning(
                "[PERFORMANCE] Handling request = {Request} took {Seconds}.{Milliseconds}",
                typeof(TRequest).Name,
                timeTaken.Seconds,
                timeTaken.Milliseconds);
        }

        logger.LogInformation(
            "[END] Handled {Request} with {Response}",
            typeof(TRequest).Name,
            ToFriendlyTypeName(typeof(TResponse)));
        return response;
    }


    private string ToFriendlyTypeName(Type t)
    {
        if (t.IsGenericType)
        {
            string typeName = t.Name.Substring(0, t.Name.IndexOf('`'));

            var genericArguments = t.GetGenericArguments().Select(ToFriendlyTypeName).ToList();

            return $"{typeName}<{string.Join(", ", genericArguments)}>";
        }

        if (t.IsArray)
            return $"{ToFriendlyTypeName(t.GetElementType()!)}[]";

        return t.Name;
    }
}