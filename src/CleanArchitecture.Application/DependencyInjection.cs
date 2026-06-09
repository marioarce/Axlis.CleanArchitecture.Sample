using System.Reflection;
using CleanArchitecture.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

/// <summary>
/// Registration entry point for the Application layer.
/// Wires MediatR (with the logging pipeline behavior) and FluentValidation validators.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        // TODO(Features): once the PowerCSharp Features framework is available, application-level
        // cross-cutting behaviors (validation, caching, etc.) may be contributed via Built-in Features.

        return services;
    }
}
