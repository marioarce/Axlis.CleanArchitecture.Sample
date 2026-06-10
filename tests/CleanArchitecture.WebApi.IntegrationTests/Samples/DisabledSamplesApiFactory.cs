using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.WebApi.IntegrationTests.Samples;

/// <summary>
/// Test host that explicitly forces the <c>Samples</c> feature flag off, to verify the
/// <c>[FeatureGate]</c> hides the endpoint (404) when the feature is disabled.
/// </summary>
public sealed class DisabledSamplesApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PowerFeatures:Samples:Enabled"] = "false",
            });
        });
    }
}
