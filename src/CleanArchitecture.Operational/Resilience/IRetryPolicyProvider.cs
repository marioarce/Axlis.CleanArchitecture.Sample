using Polly;

namespace CleanArchitecture.Operational.Resilience;

/// <summary>
/// Provides reusable resilience pipelines (retry, timeout, etc.) for operational
/// cross-cutting concerns. Implemented with Polly and consumed by Infrastructure adapters.
/// </summary>
public interface IRetryPolicyProvider
{
    /// <summary>
    /// Gets a default retry pipeline with exponential backoff.
    /// </summary>
    ResiliencePipeline GetDefaultPipeline();
}
