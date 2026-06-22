using CleanArchitecture.Application.Api.Shared.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PowerCSharp.Feature.Cache.Abstractions;

namespace CleanArchitecture.Application.Api.Shared.Data.Samples;

/// <summary>
/// Factory context for the disk cache sample. Adds <see cref="IDiskCacheService"/> (provided by the host's
/// configured disk cache provider) on top of the base collaborators.
/// </summary>
public sealed class SampleDiskCacheFactoryContext : BaseFactoryContext
{
    public SampleDiskCacheFactoryContext(
        ILogger<SampleDiskCacheFactoryContext> logger,
        IConfiguration configuration,
        IDiskCacheService diskCache)
        : base(logger, configuration)
    {
        DiskCache = diskCache;
    }

    /// <summary>The disk cache abstraction used by the sample.</summary>
    public IDiskCacheService DiskCache { get; }
}
