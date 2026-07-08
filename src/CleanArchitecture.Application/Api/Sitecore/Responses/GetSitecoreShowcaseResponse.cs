namespace CleanArchitecture.Application.Api.Sitecore.Responses;

/// <summary>
/// Response returned by the Sitecore showcase endpoint.
/// Each pivot is populated when the corresponding item is found; null otherwise.
/// </summary>
public sealed class GetSitecoreShowcaseResponse
{
    /// <summary>Home item pivot.</summary>
    public HomePivot? Home { get; init; }

    /// <summary>PAxes traversal result.</summary>
    public AxesPivot? Axes { get; init; }

    /// <summary>GetDescendants result (expensive — for testing only).</summary>
    public DescendantsPivot? Descendants { get; init; }

    /// <summary>Rich-API example: item Metadata and DiagnosticsData from WithResult.</summary>
    public RichApiPivot? RichApi { get; init; }
}

/// <summary>Home item pivot.</summary>
public sealed class HomePivot
{
    public string? Title { get; init; }
    public string? Text { get; init; }
}

/// <summary>Axes pivot output.</summary>
public sealed class AxesPivot
{
    public string? ItemPath { get; init; }
    public string? ParentPath { get; init; }
    public int ChildrenCount { get; init; }
    public string? GrandparentPath { get; init; }
    public int SiblingsCount { get; init; }
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
