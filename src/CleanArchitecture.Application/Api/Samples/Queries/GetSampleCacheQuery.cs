using Ardalis.Result;
using CleanArchitecture.Application.Api.Samples.Responses;
using MediatR;

namespace CleanArchitecture.Application.Api.Samples.Queries;

/// <summary>
/// Query that returns a cached sample value for the given key, demonstrating
/// PowerCSharp.Feature.Cache (BitFaster) via <c>ICacheService</c>.
/// </summary>
public sealed class GetSampleCacheQuery : IRequest<Result<GetSampleCacheResponse>>
{
    public GetSampleCacheQuery(string key)
    {
        Key = key;
    }

    /// <summary>The cache key to read or populate.</summary>
    public string Key { get; }
}
