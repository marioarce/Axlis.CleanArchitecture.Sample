using System.Reflection;
using CleanArchitecture.Application.Api.Shared.Data.Samples;
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

        // Factory contexts (the cache sample). Scoped so per-request collaborators flow in;
        // ICacheService itself is supplied by the host's configured cache provider.
        services.AddScoped<SampleCacheFactoryContext>();

        return services;
    }
}
