using CleanArchitecture.Application.Abstractions;

namespace CleanArchitecture.Infrastructure.Services;

/// <summary>
/// Infrastructure adapter implementing the Application's <see cref="IDateTimeProvider"/> port.
/// Demonstrates the Dependency Inversion Principle: Infrastructure depends on Application
/// (not the other way around) and provides the concrete implementation.
/// </summary>
public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
