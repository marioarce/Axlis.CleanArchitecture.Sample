using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Operational;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;

/// <summary>
/// Registration entry point for the Infrastructure layer.
/// Binds Application ports (abstractions) to their concrete adapters and wires
/// operational concerns. This is where external/third-party integrations live.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Bind Application port -> Infrastructure adapter (Dependency Inversion).
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // Operational cross-cutting concerns (resilience pipelines, etc.).
        services.AddOperational();

        // TODO(Features): third-party-backed capabilities (cache, secrets, observability,
        // external APIs) will be provided as PowerCSharp Pluggable Features (PowerCSharp.Feature.*)
        // and registered here once those packages are implemented and consumed via the local NuGet feed.

        return services;
    }
}
