using CleanArchitecture.Application.Api.Samples.Queries;
using CleanArchitecture.Application.Api.Samples.Responses;
using CleanArchitecture.Presentation.Api;
using CleanArchitecture.Presentation.Api.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Presentation.Controllers.V1;

/// <summary>
/// Living-documentation endpoints that demonstrate how to use the PowerCSharp Features.
/// The whole controller is gated behind the <c>Samples</c> feature flag, so it is hidden
/// unless <c>PowerFeatures:Samples:Enabled</c> is true.
/// </summary>
[ApiController]
[Route("v1/samples")]
[FeatureGate("Samples")]
public sealed class SamplesController : BaseApiController
{
    public SamplesController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<SamplesController> logger)
        : base(mediator, httpContextAccessor, logger)
    {
    }

    /// <summary>
    /// Demonstrates <c>PowerCSharp.Feature.Cache.BitFaster</c> via <c>ICacheService</c>.
    /// </summary>
    /// <remarks>
    /// The first call for a given <paramref name="key"/> is a cache miss (slow, simulated work);
    /// subsequent calls are cache hits (fast). The response exposes <c>cacheHit</c> and
    /// <c>elapsedMs</c> so the behavior is observable in Swagger.
    /// </remarks>
    /// <param name="key">The cache key to read or populate.</param>
    [HttpGet("cache")]
    [ProducesResponseType(typeof(ApiResponse<GetSampleCacheResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public Task<IActionResult> GetCacheSampleAsync([FromQuery] string key = "sample")
        => SendAsync(new GetSampleCacheQuery(key));
}
