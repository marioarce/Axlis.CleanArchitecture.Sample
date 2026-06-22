using CleanArchitecture.Application.Api.Shared.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PowerCSharp.Feature.Cache.Abstractions;

namespace CleanArchitecture.Application.Api.Shared.Data.Samples;

/// <summary>
/// Factory context for the cache sample. Adds <see cref="ICacheService"/> (provided by the host's
/// configured cache provider, e.g. BitFaster) on top of the base collaborators.
/// </summary>
public sealed class SampleCacheFactoryContext : BaseFactoryContext
{
    public SampleCacheFactoryContext(
        ILogger<SampleCacheFactoryContext> logger,
        IConfiguration configuration,
        ICacheService cache)
        : base(logger, configuration)
    {
        Cache = cache;
    }

    /// <summary>The cache abstraction used by the sample.</summary>
    public ICacheService Cache { get; }
}
