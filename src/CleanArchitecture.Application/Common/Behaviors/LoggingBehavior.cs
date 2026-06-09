using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs the name and elapsed time of every request.
/// A cross-cutting concern applied uniformly to all handlers without polluting them.
/// </summary>
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).FullName;

        _logger.LogInformation("Handling request '{RequestName}'", requestName);

        var timer = Stopwatch.StartNew();

        var response = await next().ConfigureAwait(false);

        timer.Stop();

        _logger.LogInformation(
            "Handled request '{RequestName}' in {ElapsedMilliseconds} ms",
            requestName,
            timer.ElapsedMilliseconds);

        return response;
    }
}
