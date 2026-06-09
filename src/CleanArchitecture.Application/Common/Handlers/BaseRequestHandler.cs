using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Common.Handlers;

/// <summary>
/// Optional base class for MediatR request handlers, providing a logger and a
/// consistent construction hook. Concrete handlers implement <see cref="Handle"/>.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected ILogger Logger { get; }

    protected BaseRequestHandler(ILogger logger)
    {
        Logger = logger;
        Logger.LogDebug("Initialized handler {HandlerType}", GetType().FullName);
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
