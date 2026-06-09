namespace CleanArchitecture.Shared.Enums;

/// <summary>
/// Generic, application-agnostic sort direction used by query/paging contracts.
/// The Shared kernel holds cross-cutting primitives with no third-party dependencies.
/// </summary>
public enum SortDirection
{
    Ascending = 0,
    Descending = 1,
}
