using System.Net;
using System.Text.Json;

namespace CleanArchitecture.WebApi.IntegrationTests.Samples;

/// <summary>
/// Proves the cache sample works end-to-end: the first call for a key is a miss, the second is a hit
/// (served by PowerCSharp.Feature.Cache.BitFaster), and the gate hides the endpoint when disabled.
/// </summary>
public sealed class SamplesCacheEndpointTests : IClassFixture<SamplesApiFactory>
{
    private readonly SamplesApiFactory _factory;

    public SamplesCacheEndpointTests(SamplesApiFactory factory) => _factory = factory;

    [Fact]
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

    [Fact]
    public async Task Gate_Hides_Endpoint_When_Feature_Disabled()
    {
        // A client whose configuration leaves Samples disabled must get 404.
        using var disabledFactory = new DisabledSamplesApiFactory();
        var client = disabledFactory.CreateClient();

        var response = await client.GetAsync("/v1/samples/cache?key=anything");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

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

    private sealed record SampleData(string? Value, bool CacheHit);
}
