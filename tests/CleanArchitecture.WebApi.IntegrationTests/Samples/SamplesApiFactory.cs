using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.WebApi.IntegrationTests.Samples;

/// <summary>
/// Test host for the samples endpoints that forces the <c>Samples</c> feature flag on,
/// independent of environment, so the gated controller is reachable.
/// </summary>
public sealed class SamplesApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PowerFeatures:Samples:Enabled"] = "true",
            });
        });
    }
}
