namespace CleanArchitecture.Domain.Common;

/// <summary>
/// Marker interface identifying an aggregate root — the only entities a repository
/// should load and persist directly. Apply to your domain aggregates.
/// </summary>
public interface IAggregateRoot
{
}
