using CleanArchitecture.Application.Api.Shared.Responses;

namespace CleanArchitecture.Application.Api.Samples.Responses;

/// <summary>
/// Response for the cache sample. The flags make the cache behavior observable in Swagger:
/// the first call is a miss (slow), the second is a hit (fast).
/// </summary>
public sealed class GetSampleCacheResponse : IResponse
{
    /// <summary>The cache key used.</summary>
    public string Key { get; init; } = string.Empty;

    /// <summary>The cached or freshly built value.</summary>
    public string? Value { get; init; }

    /// <summary>True when the value was served from cache; false when it was just built.</summary>
    public bool CacheHit { get; init; }

    /// <summary>How long the operation took, in milliseconds.</summary>
    public long ElapsedMs { get; init; }
}
