using System.Diagnostics;
using CleanArchitecture.Application.Api.Samples.Responses;
using CleanArchitecture.Application.Api.Shared.Factories;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Api.Shared.Data.Samples;

/// <summary>
/// Builds/fetches the cache sample value. On a cache miss it simulates expensive work, stores the
/// result via <c>ICacheService</c>, and reports timing so the cache effect is observable.
/// </summary>
public sealed class SampleCacheFactory : BaseFactory<SampleCacheFactoryContext>
{
    private static readonly TimeSpan SimulatedWork = TimeSpan.FromMilliseconds(750);

    public SampleCacheFactory(SampleCacheFactoryContext context)
        : base(context)
    {
    }

    /// <summary>Returns the cached value for <paramref name="key"/>, building it on a miss.</summary>
    public async Task<GetSampleCacheResponse> GetOrCreateAsync(string key, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        if (Context.Cache.TryGet<string>(key, out var cached))
        {
            stopwatch.Stop();
            Context.Logger.LogInformation("Cache HIT for sample key {Key}", key);

            return new GetSampleCacheResponse
            {
                Key = key,
                Value = cached,
                CacheHit = true,
                ElapsedMs = stopwatch.ElapsedMilliseconds,
            };
        }

        Context.Logger.LogInformation("Cache MISS for sample key {Key}; building value", key);
        await Task.Delay(SimulatedWork, cancellationToken).ConfigureAwait(false);

        var value = $"sample-value-for:{key}@{DateTimeOffset.UtcNow:O}";
        Context.Cache.Set(key, value);

        stopwatch.Stop();
        return new GetSampleCacheResponse
        {
            Key = key,
            Value = value,
            CacheHit = false,
            ElapsedMs = stopwatch.ElapsedMilliseconds,
        };
    }

    /// <summary>Returns a snapshot of the cache contents (key count and the keys).</summary>
    public GetSampleCacheStatusResponse GetStatus()
    {
        var keys = Context.Cache.GetKeys();
        return new GetSampleCacheStatusResponse
        {
            Count = keys.Count,
            Keys = keys,
        };
    }

    /// <summary>Clears the cache and returns the number of keys that were present.</summary>
    public int Clear()
    {
        var clearedKeys = Context.Cache.GetKeys().Count;
        Context.Cache.Clear();
        Context.Logger.LogInformation("Cleared sample cache ({ClearedKeys} keys removed)", clearedKeys);
        return clearedKeys;
    }
}
