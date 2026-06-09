namespace CleanArchitecture.Application.Abstractions;

/// <summary>
/// A port (abstraction) the Application layer depends on for the current time.
/// Following the Dependency Inversion Principle, the interface lives in the inner
/// (Application) layer and the concrete adapter is implemented in Infrastructure
/// and supplied at runtime by the composition root (WebApi).
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC time.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
