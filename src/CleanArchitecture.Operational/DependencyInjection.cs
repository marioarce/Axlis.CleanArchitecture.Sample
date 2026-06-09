using CleanArchitecture.Operational.Resilience;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Operational;

/// <summary>
/// Registration entry point for operational cross-cutting concerns
/// (resilience, diagnostics). Referenced by Infrastructure.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddOperational(this IServiceCollection services)
    {
        services.AddSingleton<IRetryPolicyProvider, RetryPolicyProvider>();

        return services;
    }
}
