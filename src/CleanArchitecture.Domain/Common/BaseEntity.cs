namespace CleanArchitecture.Domain.Common;

/// <summary>
/// Base type for domain entities, providing a strongly-typed identifier.
/// Domain is the innermost layer and has no dependencies on other layers.
/// </summary>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
public abstract class BaseEntity<TId>
    where TId : notnull
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public TId Id { get; protected set; } = default!;
}
