using Ardalis.Result;
using Axlis.Services;
using CleanArchitecture.Application.Api.Sitecore.Queries;
using CleanArchitecture.Application.Api.Sitecore.Responses;
using CleanArchitecture.Application.Common.Handlers;
using CleanArchitecture.Application.Sitecore.Templates;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Api.Sitecore.Handlers;

/// <summary>
/// Handles <see cref="GetSitecoreShowcaseQuery"/>.
/// Exercises six Axlis API pivots:
/// TextField, ImageField/MultilistField, ItemReferenceField, Axes traversal,
/// GetDescendants, and the WithResult rich-API.
/// </summary>
public sealed class GetSitecoreShowcaseQueryHandler
    : BaseRequestHandler<GetSitecoreShowcaseQuery, Result<GetSitecoreShowcaseResponse>>
{
    private readonly ISitecoreFacade _facade;

    /// <summary>
    /// Initializes a new instance of <see cref="GetSitecoreShowcaseQueryHandler"/>.
    /// </summary>
    public GetSitecoreShowcaseQueryHandler(
        ILogger<GetSitecoreShowcaseQueryHandler> logger,
        ISitecoreFacade facade)
        : base(logger)
    {
        _facade = facade;
        ArgumentNullException.ThrowIfNull(_facade);
    }

    /// <inheritdoc/>
    public override async Task<Result<GetSitecoreShowcaseResponse>> Handle(
        GetSitecoreShowcaseQuery request,
        CancellationToken cancellationToken)
    {
        var rootPath = request.RootPath;
        // fixme: adjust relative paths to match the connected Sitecore content tree
        var disclaimerPath  = $"{rootPath}/disclaimer";
        var dictionaryPath  = $"{GetParentPath(rootPath)}/dictionary";

        // ── Pivot 1: TextField (Disclaimer) ───────────────────────────────────
        var disclaimer = await _facade
            .GetItemByPathAsync<Disclaimer>(disclaimerPath, cancellationToken)
            .ConfigureAwait(false);

        DisclaimerPivot? disclaimerPivot = null;
        if (disclaimer is not null)
        {
            disclaimerPivot = new DisclaimerPivot
            {
                Heading     = disclaimer.Heading.Value,
                Description = disclaimer.Description.Value,
            };
        }

        // ── Pivot 2: ImageField + MultilistField (HomePage) ───────────────────
        var home = await _facade
            .GetItemByPathAsync<HomePage>(rootPath, cancellationToken)
            .ConfigureAwait(false);

        HomePivot? homePivot = null;
        if (home is not null)
        {
            var cssLinks    = home.HeadCssLinks.As<PresentationAssetLink>();
            var firstParent = cssLinks.Count > 0
                ? cssLinks[0].Axes.Parent?.Path
                : null;

            homePivot = new HomePivot
            {
                MetaThumbnailSrc   = home.MetaThumbnail.Value?.Sources.Small,
                MetaThumbnailAlt   = home.MetaThumbnail.Value?.AlternativeText,
                HeadCssLinksCount  = cssLinks.Count,
                HeadCssLinkValues  = cssLinks.Select(l => l.Link.Value).ToList(),
                FirstLinkParentPath = firstParent,
            };
        }

        // ── Pivot 3: ItemReferenceField (DictionaryRoot) ──────────────────────
        var dictRoot = await _facade
            .GetItemByPathAsync<DictionaryRoot>(dictionaryPath, cancellationToken)
            .ConfigureAwait(false);

        DictionaryPivot? dictionaryPivot = null;
        if (dictRoot is not null)
        {
            var fallback = dictRoot.FallbackDomain.Value;
            dictionaryPivot = new DictionaryPivot
            {
                FallbackDomainId   = fallback?.Id,
                FallbackDomainPath = fallback?.Path,
            };
        }

        // ── Pivot 4: Axes traversal (on the home item) ────────────────────────
        AxesPivot? axesPivot = null;
        if (home is not null)
        {
            var parent      = home.Axes.Parent;
            var children    = home.Axes.Children;
            var grandparent = parent?.Axes.Parent;
            var siblings    = home.Axes.Siblings;
            var typedKids   = home.Axes.GetChildren<PresentationAssetLink>();

            axesPivot = new AxesPivot
            {
                ItemPath          = home.Path,
                ParentPath        = parent?.Path,
                ChildrenCount     = children?.Count ?? 0,
                GrandparentPath   = grandparent?.Path,
                SiblingsCount     = siblings?.Count ?? 0,
                TypedChildrenCount = typedKids.Count,
            };
        }

        // ── Pivot 5: GetDescendants (on the home item) ────────────────────────
        DescendantsPivot? descendantsPivot = null;
        if (home is not null)
        {
            var descendants = home.Axes.GetDescendants<Style>();
            descendantsPivot = new DescendantsPivot
            {
                RootPath         = home.Path,
                DescendantsCount = descendants.Count,
            };
        }

        // ── Pivot 6: WithResult rich API (Disclaimer) ─────────────────────────
        var richResult = await _facade
            .GetItemByPathWithResultAsync<Disclaimer>(disclaimerPath, cancellationToken)
            .ConfigureAwait(false);

        var richPivot = new RichApiPivot
        {
            Found             = richResult.HasValue,
            ItemId            = richResult.Metadata?.ItemId,
            ItemPath          = richResult.Metadata?.ItemPath,
            ItemVersion       = richResult.Metadata?.ItemVersion,
            FetchedAtMs       = richResult.Metadata?.Timestamp,
            DiagnosticsMessages = richResult.Diagnostics?.Events
                .Select(e => $"[{e.Severity}] {e.Message}")
                .ToList() ?? new List<string>(),
        };

        var response = new GetSitecoreShowcaseResponse
        {
            Disclaimer  = disclaimerPivot,
            Home        = homePivot,
            Dictionary  = dictionaryPivot,
            Axes        = axesPivot,
            Descendants = descendantsPivot,
            RichApi     = richPivot,
        };

        return Result.Success(response);
    }

    private static string GetParentPath(string path)
    {
        var trimmed = path.TrimEnd('/');
        var idx = trimmed.LastIndexOf('/');
        return idx <= 0 ? "/" : trimmed[..idx];
    }
}
