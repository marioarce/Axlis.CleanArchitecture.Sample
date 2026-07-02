namespace CleanArchitecture.Application.Api.Sitecore.Responses;

/// <summary>
/// Response returned by the Sitecore showcase endpoint.
/// Each pivot is populated when the corresponding item is found; null otherwise.
/// </summary>
public sealed class GetSitecoreShowcaseResponse
{
    /// <summary>Pivot 1 — Disclaimer TextFields.</summary>
    public DisclaimerPivot? Disclaimer { get; init; }

    /// <summary>Pivot 2 — Home page: ImageField + MultilistField + Axes.</summary>
    public HomePivot? Home { get; init; }

    /// <summary>Pivot 3 — Dictionary root: ItemReferenceField.</summary>
    public DictionaryPivot? Dictionary { get; init; }

    /// <summary>Pivot 4 — Axes traversal result.</summary>
    public AxesPivot? Axes { get; init; }

    /// <summary>Pivot 5 — GetDescendants result (expensive — for testing only).</summary>
    public DescendantsPivot? Descendants { get; init; }

    /// <summary>Rich-API example: item metadata from WithResult.</summary>
    public RichApiPivot? RichApi { get; init; }
}

/// <summary>Disclaimer pivot output.</summary>
public sealed class DisclaimerPivot
{
    public string? Heading { get; init; }
    public string? Description { get; init; }
}

/// <summary>Home page pivot output.</summary>
public sealed class HomePivot
{
    public string? MetaThumbnailSrc { get; init; }
    public string? MetaThumbnailAlt { get; init; }
    public int HeadCssLinksCount { get; init; }
    public List<string?> HeadCssLinkValues { get; init; } = new();
    public string? FirstLinkParentPath { get; init; }
}

/// <summary>Dictionary pivot output.</summary>
public sealed class DictionaryPivot
{
    public string? FallbackDomainId { get; init; }
    public string? FallbackDomainPath { get; init; }
}

/// <summary>Axes pivot output.</summary>
public sealed class AxesPivot
{
    public string? ItemPath { get; init; }
    public string? ParentPath { get; init; }
    public int ChildrenCount { get; init; }
    public string? GrandparentPath { get; init; }
    public int SiblingsCount { get; init; }
    public int TypedChildrenCount { get; init; }
}

/// <summary>Descendants pivot output.</summary>
public sealed class DescendantsPivot
{
    public string? RootPath { get; init; }
    public int DescendantsCount { get; init; }
}

/// <summary>Rich API (WithResult) pivot output.</summary>
public sealed class RichApiPivot
{
    public bool Found { get; init; }
    public string? ItemId { get; init; }
    public string? ItemPath { get; init; }
    public int? ItemVersion { get; init; }
    public long? FetchedAtMs { get; init; }
    public List<string> DiagnosticsMessages { get; init; } = new();
}
