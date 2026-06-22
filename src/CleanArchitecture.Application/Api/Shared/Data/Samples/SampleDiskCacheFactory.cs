using System.Diagnostics;
using CleanArchitecture.Application.Api.Samples.Responses;
using CleanArchitecture.Application.Api.Shared.Factories;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Api.Shared.Data.Samples;

/// <summary>
/// Builds/fetches the disk cache sample value. On a cache miss it simulates expensive work, stores the
/// result via <c>IDiskCacheService</c>, and reports timing so the cache effect is observable.
/// </summary>
public sealed class SampleDiskCacheFactory : BaseFactory<SampleDiskCacheFactoryContext>
{
    private static readonly TimeSpan SimulatedWork = TimeSpan.FromMilliseconds(750);

    public SampleDiskCacheFactory(SampleDiskCacheFactoryContext context)
        : base(context)
    {
    }

    /// <summary>Returns the cached value for <paramref name="key"/>, building it on a miss.</summary>
    public async Task<GetSampleDiskCacheResponse> GetOrCreateAsync(string key, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var cached = await Context.DiskCache.GetAsync<string>(key, cancellationToken).ConfigureAwait(false);
        if (cached != null)
        {
            stopwatch.Stop();
            Context.Logger.LogInformation("Disk cache HIT for sample key {Key}", key);

            return new GetSampleDiskCacheResponse
            {
                Key = key,
                Value = cached,
                CacheHit = true,
                ElapsedMs = stopwatch.ElapsedMilliseconds,
            };
        }

        Context.Logger.LogInformation("Disk cache MISS for sample key {Key}; building value", key);
        await Task.Delay(SimulatedWork, cancellationToken).ConfigureAwait(false);

        var value = $"disk-cache-sample-for:{key}@{DateTimeOffset.UtcNow:O}";
        await Context.DiskCache.SetAsync(key, value, cancellationToken).ConfigureAwait(false);

        stopwatch.Stop();
        return new GetSampleDiskCacheResponse
        {
            Key = key,
            Value = value,
            CacheHit = false,
            ElapsedMs = stopwatch.ElapsedMilliseconds,
        };
    }

    /// <summary>Returns a snapshot of the disk cache status.</summary>
    public async Task<GetSampleDiskCacheStatusResponse> GetStatusAsync(CancellationToken cancellationToken)
    {
        // Use ICacheStore capabilities to get disk cache keys
        var keys = await Context.DiskCache.GetKeysAsync(cancellationToken).ConfigureAwait(false);
        
        Context.Logger.LogInformation("Disk cache status retrieved - {Count} keys found", keys.Count);
        
        return new GetSampleDiskCacheStatusResponse
        {
            Count = keys.Count,
            Keys = keys.ToList(),
        };
    }

    /// <summary>Clears the disk cache and returns the number of keys that were present.</summary>
    public async Task<int> ClearAsync(CancellationToken cancellationToken)
    {
        // Get keys before clearing for accurate count
        var keys = await Context.DiskCache.GetKeysAsync(cancellationToken).ConfigureAwait(false);
        var clearedKeys = keys.Count;
        
        // Use ICacheStore ClearAsync method
        await Context.DiskCache.ClearAsync(cancellationToken).ConfigureAwait(false);
        
        Context.Logger.LogInformation("Cleared disk cache ({ClearedKeys} keys removed)", clearedKeys);
        return clearedKeys;
    }
}
