using Polly;
using Polly.Retry;

namespace CleanArchitecture.Operational.Resilience;

/// <summary>
/// Default <see cref="IRetryPolicyProvider"/> backed by Polly.
/// </summary>
public sealed class RetryPolicyProvider : IRetryPolicyProvider
{
    public ResiliencePipeline GetDefaultPipeline()
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(200),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
            })
            .Build();
    }
}
