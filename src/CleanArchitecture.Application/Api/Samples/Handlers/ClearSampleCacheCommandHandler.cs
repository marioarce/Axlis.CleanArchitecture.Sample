using Ardalis.Result;
using CleanArchitecture.Application.Api.Samples.Commands;
using CleanArchitecture.Application.Api.Shared.Data.Samples;
using CleanArchitecture.Application.Common.Handlers;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Api.Samples.Handlers;

/// <summary>Orchestrates <see cref="ClearSampleCacheCommand"/> by delegating the clear to the factory.</summary>
public sealed class ClearSampleCacheCommandHandler
    : BaseRequestHandler<ClearSampleCacheCommand, Result>
{
    private readonly SampleCacheFactoryContext _factoryContext;

    public ClearSampleCacheCommandHandler(
        ILogger<ClearSampleCacheCommandHandler> logger,
        SampleCacheFactoryContext factoryContext)
        : base(logger)
    {
        _factoryContext = factoryContext;
        ArgumentNullException.ThrowIfNull(_factoryContext);
    }

    public override Task<Result> Handle(ClearSampleCacheCommand request, CancellationToken cancellationToken)
    {
        var factory = new SampleCacheFactory(_factoryContext);
        factory.Clear();
        return Task.FromResult(Result.NoContent());
    }
}
