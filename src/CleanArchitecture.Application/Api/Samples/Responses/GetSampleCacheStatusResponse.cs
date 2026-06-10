using CleanArchitecture.Application.Api.Shared.Responses;

namespace CleanArchitecture.Application.Api.Samples.Responses;

/// <summary>
/// Response for the cache status sample: how many entries are cached and which keys.
/// </summary>
public sealed class GetSampleCacheStatusResponse : IResponse
{
    /// <summary>The number of keys currently stored.</summary>
    public int Count { get; init; }

    /// <summary>The keys currently stored in the cache.</summary>
    public IReadOnlyCollection<string> Keys { get; init; } = Array.Empty<string>();
}
