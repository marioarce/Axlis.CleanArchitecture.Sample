using Ardalis.Result;
using Axlis.Services;
using CleanArchitecture.Application.Api.Sitecore.Queries;
using CleanArchitecture.Application.Api.Sitecore.Responses;
using CleanArchitecture.Application.Common.Handlers;
using CleanArchitecture.Application.Sitecore.Templates.Sample;
using CleanArchitecture.Application.Sitecore.Templates.System;
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
        /*
         * /sitecore/content/Home
         */
        var homePath = "/sitecore/content/Home";

        /*
         * /sitecore/system/Languages
         */
        var languagesPath = "/sitecore/system/Languages";

        /*
         * /sitecore/system/Publishing targets
         */
        var publishingTargetsPath = "/sitecore/system/Publishing targets";

        // ── Example: Home, how to get item by Sitecore Path ───────────────────────────────────
        var homeItem = await _facade
            .GetItemByPathAsync<SampleItem>(homePath, cancellationToken)
            .ConfigureAwait(false);

        HomePivot? homePivot = null;

        if (homeItem is not null)
        {
            /* How to access a TextField value
             */
            homePivot = new HomePivot
            {
                Title = homeItem.Title.Value,
                Text = homeItem.Text.Value,
            };
        }

        // ── Example: Axes traversal ────────────────────────
        AxesPivot? axesPivot = null;

        if (homeItem is not null)
        {
            var parent = homeItem.Axes.Parent;
            var children = homeItem.Axes.Children;
            var grandparent = parent?.Axes.Parent;
            var siblings = homeItem.Axes.Siblings;

            axesPivot = new AxesPivot
            {
                ItemPath = homeItem.Path,
                ParentPath = parent?.Path ?? "unknown",
                ChildrenCount = children?.Count ?? 0,
                GrandparentPath = grandparent?.Path ?? "unknown",
                SiblingsCount = siblings?.Count ?? 0
            };
        }

        // ── Example: Languages ───────────────────
        var languagesItem = await _facade
            .GetItemByPathAsync<Node>(languagesPath, cancellationToken)
            .ConfigureAwait(false);

        // ── Example: GetDescendants  ────────────────────────
        DescendantsPivot? descendantsPivot = null;

        if (languagesItem is not null)
        {
            var descendants = languagesItem.Axes.GetDescendants<Language>();

            descendantsPivot = new DescendantsPivot
            {
                RootPath = languagesItem.Path,
                DescendantsCount = descendants.Count,
            };
        }

        // ── Example: how to get item by path rich API result (Metadata + DiagnosticsData) ─────────────────────────
        var richResult = await _facade
            .GetItemByPathWithResultAsync<Node>(publishingTargetsPath, cancellationToken)
            .ConfigureAwait(false);

        var richPivot = new RichApiPivot
        {
            Found = richResult.HasValue,
            ItemId = richResult.Metadata?.ItemId,
            ItemPath = richResult.Metadata?.ItemPath,
            ItemVersion = richResult.Metadata?.ItemVersion,
            FetchedAtMs = richResult.Metadata?.Timestamp,
            DiagnosticsMessages = richResult.Diagnostics?.Events
                .Select(e => $"[{e.Severity}] {e.Message}")
                .ToList() ?? new List<string>(),
        };

        var response = new GetSitecoreShowcaseResponse
        {
            Home = homePivot,
            Axes = axesPivot,
            Descendants = descendantsPivot,
            RichApi = richPivot,
        };

        return Result.Success(response);
    }
}
