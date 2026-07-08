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
    /* DevNote:
     * ISitecoreFacade is the main entry point for Axlis. It provides a clean API for
     * fetching Sitecore items via GraphQL without dealing with HTTP, JSON, or query
     * construction directly. The facade handles caching, lazy loading, and error handling
     * transparently. To use Axlis in your project: 1) Install Axlis and Axlis.GraphQL packages,
     * 2) Configure services with AddAxlis() and AddAxlisGraphQL() in Program.cs, 3) Call
     * UseAxlis() after building the app to wire the ambient lazy-loader for Axes traversal.
     * See /Users/mario.arce/Downloads/git-marioarce/Axlis/docs/GettingStarted.md for setup details.
     */
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
        /* DevNote:
         * GetItemByPathAsync<T> fetches a Sitecore item by its path and maps it to your
         * strongly-typed template POCO. The generic parameter T must be a class that inherits
         * from ExtendedItem and is decorated with [SitecoreTemplate] attribute. Axlis uses
         * reflection to create an instance, then populates it with the fetched item data.
         * The method returns T? (null if not found) and never throws - errors are logged
         * internally. This is the "Clean" API flavor. For diagnostics, use GetItemByPathWithResultAsync<T>
         * which returns AxlisResult<T> with metadata and diagnostic events.
         */
        // Use the provided root path or default to site content
        var homePath = $"{request.RootPath}/Home";

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
            /* DevNote:
             * Template POCOs define Sitecore templates as strongly-typed C# classes. Each field
             * property calls GetField<TField>("Sitecore field name") and returns the appropriate
             * field type (TextField, ImageField, MultilistField, etc.). Field names are case-insensitive
             * at lookup time. Access field values via the RawValue property (e.g., TextField.RawValue)
             * or type-specific properties (e.g., ImageField.Src, ImageField.Alt). All field types
             * inherit from BaseField and expose FieldName, RawValue, and IsEmpty. To define your own
             * templates: inherit from ExtendedItem, decorate with [SitecoreTemplate("{GUID}")],
             * and add field properties with [SitecoreField] attributes. See Templates.md for field types.
             */
            homePivot = new HomePivot
            {
                Title = homeItem.Title.Value,
                Text = homeItem.Text.Value,
            };
        }

        // ── Example: Axes traversal ────────────────────────
        /* DevNote:
         * Axes (exposed as ExtendedItem.Axes) provides Synthesis-style tree traversal over
         * Headless GraphQL items. Available members: Parent (IItem?), Children (IEnumerable<IItem>),
         * Siblings (IEnumerable<IItem>), GetChildren(predicate?), and GetDescendants(predicate?).
         * GetItemByPathAsync fetches a full item graph with 2 levels of ancestry and first 50 children,
         * so accessing Parent or Children typically triggers no extra round-trip. However, accessing
         * data beyond what was fetched (e.g., Parent.Parent.Parent or >50 children) triggers a
         * lazy-fetch via SitecoreItemLazyLoader. Lazy fetches are synchronous (property getter cannot
         * be async), which is safe in ASP.NET Core but avoid deep traversal in non-ASP environments.
         * For performance-critical scenarios, use GetItemsByPathsAsync for batch pre-fetch instead.
         * See Axes.md for detailed lazy-loading behavior and usage patterns.
         */
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
        /* DevNote:
         * GetDescendants(predicate?) returns all descendants depth-first, optionally filtered
         * by a predicate. This can be expensive for large trees - use with caution. The generic
         * overload GetDescendants<T>() filters by template type automatically. When you need
         * to fetch many items at once (e.g., navigation menus), prefer GetItemsByPathsAsync
         * over individual fetches + Axes traversal. GetItemsByPathsAsync issues one batched
         * GraphQL request with aliased sub-queries, which is more efficient than multiple
         * round-trips. For scenarios where you only need field values and never touch Axes,
         * use GetItemFlatAsync<T> for a lighter GraphQL query with no parent/children data.
         */
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
        /* DevNote:
         * GetItemByPathWithResultAsync<T> is the "Rich" API flavor that returns AxlisResult<T>
         * containing Value, Metadata, and Diagnostics. Use this when you need provenance (item ID,
         * path, version, timestamp) or want to inspect diagnostic events (warnings, errors, info).
         * Metadata includes ItemId, ItemPath, ItemVersion, and Timestamp. Diagnostics contains a list
         * of events with Severity and Message, useful for troubleshooting GraphQL queries or caching
         * behavior. The HasValue property indicates whether the fetch succeeded. This is the
         * diagnostics-aware counterpart to the Clean API (GetItemByPathAsync<T>). Configure EnableDiagnostics
         * in AddAxlis options to control diagnostic collection (default: true). Disable for high-throughput
         * paths where diagnostics overhead is undesirable.
         */
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

        /* DevNote:
         * Caching: Axlis caching is powered by ICacheService from PowerCSharp.Feature.Cache.Abstractions.
         * SitecoreItemCacheManager sits between the facade and the GraphQL service, providing dual-indexing
         * (by ID and path) and null-safety (null results are never cached, so re-published items are picked
         * up on next request). The default AddAxlis() registration installs a NoOpCacheService (no caching).
         * For production, register a real provider like BitFaster (AddBitFasterCache()) before AddAxlis().
         * Configure CacheTtl in AddAxlis options (default: 60 minutes). Call InvalidateAsync(key) on
         * SitecoreItemCacheManager to evict specific items (e.g., on publish webhooks). Lazy-loader cache
         * routes through the same manager, so Axes traversal benefits from caching too. See Caching.md for
         * provider options and invalidation patterns.
         */
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
