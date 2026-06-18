using Ardalis.Result;
using CleanArchitecture.Application.Api.Samples.Queries;
using CleanArchitecture.Application.Api.Samples.Responses;
using CleanArchitecture.Application.Api.Shared.Data.Samples;
using CleanArchitecture.Application.Common.Handlers;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Api.Samples.Handlers;

/// <summary>Orchestrates <see cref="GetSampleCacheStatusQuery"/> by delegating to the factory.</summary>
public sealed class GetSampleCacheStatusQueryHandler
    : BaseRequestHandler<GetSampleCacheStatusQuery, Result<GetSampleCacheStatusResponse>>
{
    private readonly SampleCacheFactoryContext _factoryContext;

    public GetSampleCacheStatusQueryHandler(
        ILogger<GetSampleCacheStatusQueryHandler> logger,
        SampleCacheFactoryContext factoryContext)
        : base(logger)
    {
        _factoryContext = factoryContext;
        ArgumentNullException.ThrowIfNull(_factoryContext);
    }

    public override Task<Result<GetSampleCacheStatusResponse>> Handle(
        GetSampleCacheStatusQuery request,
        CancellationToken cancellationToken)
    {
        var factory = new SampleCacheFactory(_factoryContext);
        var status = factory.GetStatus();
        return Task.FromResult(Result.Success(status));
    }
}
