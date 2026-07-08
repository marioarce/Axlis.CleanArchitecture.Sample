using System.Net;
using System.Text.Json;

namespace CleanArchitecture.WebApi.IntegrationTests.Samples;

/// <summary>
/// Proves the cache sample works end-to-end: the first call for a key is a miss, the second is a hit
/// (served by PowerCSharp.Feature.Cache.BitFaster), and the gate hides the endpoint when disabled.
/// </summary>
public sealed class SamplesCacheEndpointTests : IClassFixture<SamplesApiFactory>
{
    /// <summary>
    /// Initializes a new instance of the SamplesCacheEndpointTests.
    /// </summary>
    /// <param name="factory">The API factory.</param>
    public SamplesCacheEndpointTests(SamplesApiFactory factory) => _factory = factory;

    [Fact(Skip = "PowerCSharp sample endpoints not relevant to Axlis sample project")]
    public async Task Cache_Sample_Misses_Then_Hits()
    {
        var client = _factory.CreateClient();
        var key = "it-" + Guid.NewGuid().ToString("N");

        var first = await GetSampleAsync(client, key);
        var second = await GetSampleAsync(client, key);

        Assert.False(first.CacheHit);          // cold: built and stored
        Assert.True(second.CacheHit);          // warm: served from cache
        Assert.Equal(first.Value, second.Value);
    }

    [Fact(Skip = "PowerCSharp sample endpoints not relevant to Axlis sample project")]
    public async Task Status_Shows_Keys_And_Delete_Clears_Cache()
    {
        var client = _factory.CreateClient();
        var key = "status-" + Guid.NewGuid().ToString("N");

        // Populate a known key.
        await GetSampleAsync(client, key);

        // Status should now include the key.
        var (countAfterAdd, keysAfterAdd) = await GetStatusAsync(client);
        Assert.True(countAfterAdd >= 1);
        Assert.Contains(key, keysAfterAdd);

        // Clear the cache.
        var deleteResponse = await client.DeleteAsync("/v1/samples/cache");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Status should now be empty.
        var (countAfterClear, keysAfterClear) = await GetStatusAsync(client);
        Assert.Equal(0, countAfterClear);
        Assert.Empty(keysAfterClear);
    }

    [Fact(Skip = "PowerCSharp sample endpoints not relevant to Axlis sample project")]
    public async Task Gate_Hides_Endpoint_When_Feature_Disabled()
    {
        // A client whose configuration leaves Samples disabled must get 404.
        using var disabledFactory = new DisabledSamplesApiFactory();
        var client = disabledFactory.CreateClient();

        var response = await client.GetAsync("/v1/samples/cache?key=anything");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // Private fields and methods (moved to end)
    private readonly SamplesApiFactory _factory;

    private static async Task<SampleData> GetSampleAsync(HttpClient client, string key)
    {
        var response = await client.GetAsync($"/v1/samples/cache?key={key}");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var data = document.RootElement.GetProperty("data");
        return new SampleData(
            data.GetProperty("value").GetString(),
            data.GetProperty("cacheHit").GetBoolean());
    }

    private static async Task<(int Count, IReadOnlyList<string> Keys)> GetStatusAsync(HttpClient client)
    {
        var response = await client.GetAsync("/v1/samples/cache/status");
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var data = document.RootElement.GetProperty("data");
        var count = data.GetProperty("count").GetInt32();
        var keys = data.GetProperty("keys")
            .EnumerateArray()
            .Select(element => element.GetString()!)
            .ToList();

        return (count, keys);
    }

    private sealed record SampleData(string? Value, bool CacheHit);
}
