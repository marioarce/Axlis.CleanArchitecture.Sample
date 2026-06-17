using CleanArchitecture.Application.Api.Samples.Commands;
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
/// <remarks>
/// <para><b>Security note:</b> the feature flag only hides these endpoints; it is <b>not</b> an
/// authorization mechanism. Some endpoints here (e.g. cache status and clear) expose or mutate
/// shared server state and are unsafe to leave open. In a real application, guard such endpoints
/// with proper authentication/authorization (e.g. <c>[Authorize]</c> with an admin policy),
/// network restrictions, and/or rate limiting — do not rely on the flag alone.</para>
/// </remarks>
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

    /// <summary>
    /// Shows the current contents of the sample cache (key count and the stored keys),
    /// demonstrating <c>ICacheService.GetKeys()</c>.
    /// </summary>
    /// <remarks>
    /// <para><b>Protect in production:</b> this leaks internal cache keys, which can reveal
    /// implementation details, identifiers, or usage patterns. In a real application, restrict it
    /// to administrators via authorization (e.g. <c>[Authorize(Policy = "Admin")]</c>) and consider
    /// network/IP restrictions. The <c>Samples</c> feature flag is for hiding the demo, not for access control.</para>
    /// </remarks>
    [HttpGet("cache/status")]
    [ProducesResponseType(typeof(ApiResponse<GetSampleCacheStatusResponse>), StatusCodes.Status200OK)]
    public Task<IActionResult> GetCacheStatusAsync()
        => SendAsync(new GetSampleCacheStatusQuery());

    /// <summary>
    /// Clears the sample cache, demonstrating <c>ICacheService.Clear()</c>. Returns 204 No Content.
    /// </summary>
    /// <remarks>
    /// <para><b>Destructive — protect in production:</b> this wipes all cached entries and can cause a
    /// cache-stampede / performance hit as data is rebuilt, and could be abused as a denial-of-service
    /// vector if left open. In a real application, require authorization (e.g.
    /// <c>[Authorize(Policy = "Admin")]</c>), and add rate limiting and audit logging. The <c>Samples</c>
    /// feature flag only hides the demo; it does not authorize the caller.</para>
    /// </remarks>
    [HttpDelete("cache")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> ClearCacheAsync()
        => SendAsync(new ClearSampleCacheCommand());

    /// <summary>
    /// Demonstrates <c>PowerCSharp.Feature.Cache.Disk</c> via <c>IDiskCacheService</c>.
    /// </summary>
    /// <remarks>
    /// The first call for a given <paramref name="key"/> is a cache miss (slow, simulated work);
    /// subsequent calls are cache hits (fast). The response exposes <c>cacheHit</c> and
    /// <c>elapsedMs</c> so the behavior is observable in Swagger.
    /// </remarks>
    /// <param name="key">The cache key to read or populate.</param>
    [HttpGet("disk-cache")]
    [FeatureGate("DiskCache")]
    [ProducesResponseType(typeof(ApiResponse<GetSampleDiskCacheResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public Task<IActionResult> GetDiskCacheSampleAsync([FromQuery] string key = "sample")
        => SendAsync(new GetSampleDiskCacheQuery(key));

    /// <summary>
    /// Shows the current contents of the sample disk cache (key count and the stored keys),
    /// demonstrating <c>IDiskCacheService.GetKeys()</c>.
    /// </summary>
    /// <remarks>
    /// <para><b>Protect in production:</b> this leaks internal cache keys, which can reveal
    /// implementation details, identifiers, or usage patterns. In a real application, restrict it
    /// to administrators via authorization (e.g. <c>[Authorize(Policy = "Admin")]</c>) and consider
    /// network/IP restrictions. The <c>DiskCache</c> feature flag is for hiding the demo, not for access control.</para>
    /// </remarks>
    [HttpGet("disk-cache/status")]
    [FeatureGate("DiskCache")]
    [ProducesResponseType(typeof(ApiResponse<GetSampleDiskCacheStatusResponse>), StatusCodes.Status200OK)]
    public Task<IActionResult> GetDiskCacheStatusAsync()
        => SendAsync(new GetSampleDiskCacheStatusQuery());

    /// <summary>
    /// Clears the sample disk cache, demonstrating <c>IDiskCacheService.Clear()</c>. Returns 204 No Content.
    /// </summary>
    /// <remarks>
    /// <para><b>Destructive — protect in production:</b> this wipes all cached entries and can cause a
    /// cache-stampade / performance hit as data is rebuilt, and could be abused as a denial-of-service
    /// vector if left open. In a real application, require authorization (e.g.
    /// <c>[Authorize(Policy = "Admin")]</c>), and add rate limiting and audit logging. The <c>DiskCache</c>
    /// feature flag only hides the demo; it does not authorize the caller.</para>
    /// </remarks>
    [HttpDelete("disk-cache")]
    [FeatureGate("DiskCache")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<IActionResult> ClearDiskCacheAsync()
        => SendAsync(new ClearSampleDiskCacheCommand());
}
