using PowerCSharp.Features.Abstractions;

namespace CleanArchitecture.WebApi.Features;

/// <summary>
/// Host-local feature that represents the <c>/v1/samples</c> living-documentation area.
/// It contributes no services or middleware; its presence makes <c>Samples</c> appear in the
/// startup diagnostics matrix. The actual endpoint gating is done by the <c>[FeatureGate]</c> filter.
/// </summary>
public sealed class SamplesFeatureModule : IFeatureModule
{
    /// <summary>The feature key.</summary>
    public const string Key = "Samples";

    /// <inheritdoc />
    public string FeatureKey => Key;

    /// <inheritdoc />
    public int Order => 1000;

    /// <inheritdoc />
    public void ConfigureServices(IFeatureRegistrationContext context)
    {
        // Flag-only feature: nothing to register. Endpoints are gated via [FeatureGate("Samples")].
    }
}
