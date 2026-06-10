namespace CleanArchitecture.Application.Api.Shared.Factories;

/// <summary>
/// Base class for data factories. A factory's job is to create/build/fetch data; it stays free of
/// HTTP and MediatR concerns so it can be reused outside the CQRS pipeline.
/// </summary>
/// <typeparam name="TContext">The factory's context type.</typeparam>
public abstract class BaseFactory<TContext>
    where TContext : BaseFactoryContext
{
    protected BaseFactory(TContext context)
    {
        Context = context;
    }

    /// <summary>The factory context providing collaborators.</summary>
    protected TContext Context { get; }
}
