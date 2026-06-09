using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Presentation;

/// <summary>
/// Registration entry point for the Presentation layer (controllers, API concerns).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        // Controllers defined in this assembly are discovered by the host via
        // AddControllers().AddApplicationPart(...) in the composition root.

        return services;
    }
}
