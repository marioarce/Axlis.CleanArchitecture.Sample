using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Api.Shared.Factories;

/// <summary>
/// Base context supplying the common collaborators a factory needs (logger, configuration).
/// Feature-specific contexts derive from this and add their own dependencies.
/// </summary>
public abstract class BaseFactoryContext
{
    protected BaseFactoryContext(ILogger logger, IConfiguration configuration)
    {
        Logger = logger;
        Configuration = configuration;
    }

    /// <summary>Logger for the factory.</summary>
    public ILogger Logger { get; }

    /// <summary>Application configuration.</summary>
    public IConfiguration Configuration { get; }
}
