using CleanArchitecture.Application.Api.Sitecore.Queries;
using CleanArchitecture.Application.Api.Sitecore.Responses;
using CleanArchitecture.Presentation.Api;
using CleanArchitecture.Presentation.Api.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Presentation.Controllers.V1;

/// <summary>
/// Showcase endpoints demonstrating how to use the Axlis Sitecore Headless GraphQL ORM
/// inside a Clean Architecture application: TextFields, ImageField, MultilistField,
/// ItemReferenceField, Axes traversal, GetDescendants, and the WithResult rich API.
/// </summary>
/// <remarks>
/// Gated behind the <c>Samples</c> feature flag (<c>PowerFeatures:Samples:Enabled = true</c>).
/// Requires <c>AxlisGraphQL:Endpoint</c> and <c>AxlisGraphQL:ApiKey</c> in user-secrets.
/// </remarks>
[ApiController]
[Route("v1/sitecore")]
[FeatureGate("Samples")]
public sealed class SitecoreController : BaseApiController
{
    public SitecoreController(
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor,
        ILogger<SitecoreController> logger)
        : base(mediator, httpContextAccessor, logger)
    {
    }

    /// <summary>
    /// Runs all six Axlis showcase pivots against the configured Sitecore instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a dev-only living-documentation endpoint. Each pivot demonstrates a
    /// different Axlis feature:
    /// <list type="number">
    ///   <item><description><b>Disclaimer</b> — TextField access.</description></item>
    ///   <item><description><b>Home</b> — ImageField + MultilistField + typed child cast.</description></item>
    ///   <item><description><b>Dictionary</b> — ItemReferenceField access.</description></item>
    ///   <item><description><b>Axes</b> — Parent, Children, Grandparent, Siblings, typed GetChildren.</description></item>
    ///   <item><description><b>Descendants</b> — GetDescendants typed traversal.</description></item>
    ///   <item><description><b>RichApi</b> — WithResult envelope: value, metadata, diagnostics.</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <param name="rootPath">
    /// Sitecore path used as the home pivot root and Axes traversal starting point.
    /// Defaults to <c>/sitecore/content/home</c>.
    /// </param>
    [HttpGet("showcase")]
    [ProducesResponseType(typeof(ApiResponse<GetSitecoreShowcaseResponse>), StatusCodes.Status200OK)]
    public Task<IActionResult> GetShowcaseAsync(
        // fixme: update default path to match the connected Sitecore content tree
        [FromQuery] string rootPath = "/sitecore/content/home")
        => SendAsync(new GetSitecoreShowcaseQuery(rootPath));
}
