namespace CleanArchitecture.Application.Api.Sitecore.Responses;

/// <summary>
/// Response returned by the Sitecore showcase endpoint.
/// Each pivot is populated when the corresponding item is found; null otherwise.
/// </summary>
public sealed class GetSitecoreShowcaseResponse
{
    /// <summary>Home item pivot.</summary>
    public HomePivot? Home { get; init; }

    /// <summary>Axes traversal result.</summary>
    public AxesPivot? Axes { get; init; }

    /// <summary>GetDescendants result (expensive — for testing only).</summary>
    public DescendantsPivot? Descendants { get; init; }

    /// <summary>Rich-API example: item Metadata and DiagnosticsData from WithResult.</summary>
    public RichApiPivot? RichApi { get; init; }
}

/// <summary>
/// Represents the home item pivot data.
/// </summary>
public sealed class HomePivot
{
    /// <summary>Gets the title field value.</summary>
    public string? Title { get; init; }

    /// <summary>Gets the text field value.</summary>
    public string? Text { get; init; }
}

/// <summary>
/// Represents the Axes traversal pivot data.
/// </summary>
public sealed class AxesPivot
{
    /// <summary>Gets the item path.</summary>
    public string? ItemPath { get; init; }

    /// <summary>Gets the parent item path.</summary>
    public string? ParentPath { get; init; }

    /// <summary>Gets the count of children items.</summary>
    public int ChildrenCount { get; init; }

    /// <summary>Gets the grandparent item path.</summary>
    public string? GrandparentPath { get; init; }

    /// <summary>Gets the count of sibling items.</summary>
    public int SiblingsCount { get; init; }
}

/// <summary>
/// Represents the descendants pivot data.
/// </summary>
public sealed class DescendantsPivot
{
    /// <summary>Gets the root item path.</summary>
    public string? RootPath { get; init; }

    /// <summary>Gets the count of descendant items.</summary>
    public int DescendantsCount { get; init; }
}

/// <summary>
/// Represents the rich API (WithResult) pivot data including metadata and diagnostics.
/// </summary>
public sealed class RichApiPivot
{
    /// <summary>Gets a value indicating whether the item was found.</summary>
    public bool Found { get; init; }

    /// <summary>Gets the item ID.</summary>
    public string? ItemId { get; init; }

    /// <summary>Gets the item path.</summary>
    public string? ItemPath { get; init; }

    /// <summary>Gets the item version.</summary>
    public int? ItemVersion { get; init; }

    /// <summary>Gets the timestamp when the item was fetched (in milliseconds).</summary>
    public long? FetchedAtMs { get; init; }

    /// <summary>Gets the diagnostic messages from the fetch operation.</summary>
    public List<string> DiagnosticsMessages { get; init; } = new();
}
