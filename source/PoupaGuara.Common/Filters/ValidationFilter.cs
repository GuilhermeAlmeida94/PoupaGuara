using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PoupaGuara.Common.Filters;

// Minimal API does not run validation automatically. This generic filter
// executes IValidator<T> and short-circuits with 400 + error codes on failure.
public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is not null &&
            context.Arguments.OfType<T>().FirstOrDefault() is { } arg)
        {
            var result = await validator.ValidateAsync(arg);
            if (!result.IsValid)
            {
                var codes = result.Errors.Select(e => e.ErrorCode).Distinct().ToArray();
                return Results.BadRequest(new { errors = codes });
            }
        }

        return await next(context);
    }
}

public static class ValidationFilterExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder)
        where T : class => builder.AddEndpointFilter<ValidationFilter<T>>();
}
