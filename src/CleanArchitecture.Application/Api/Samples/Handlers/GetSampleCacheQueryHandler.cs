using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using CleanArchitecture.Application.Api.Samples.Queries;
using CleanArchitecture.Application.Api.Samples.Responses;
using CleanArchitecture.Application.Api.Shared.Data.Samples;
using CleanArchitecture.Application.Common.Handlers;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Api.Samples.Handlers;

/// <summary>
/// Orchestrates <see cref="GetSampleCacheQuery"/>: validates input, delegates the data work to
/// <see cref="SampleCacheFactory"/>, and shapes the result. Stays free of HTTP concerns.
/// </summary>
public sealed class GetSampleCacheQueryHandler
    : BaseRequestHandler<GetSampleCacheQuery, Result<GetSampleCacheResponse>>
{
    private readonly IValidator<GetSampleCacheQuery> _validator;
    private readonly SampleCacheFactoryContext _factoryContext;

    public GetSampleCacheQueryHandler(
        ILogger<GetSampleCacheQueryHandler> logger,
        IValidator<GetSampleCacheQuery> validator,
        SampleCacheFactoryContext factoryContext)
        : base(logger)
    {
        _validator = validator;
        _factoryContext = factoryContext;

        ArgumentNullException.ThrowIfNull(_validator);
        ArgumentNullException.ThrowIfNull(_factoryContext);
    }

    public override async Task<Result<GetSampleCacheResponse>> Handle(
        GetSampleCacheQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator
            .ValidateAsync(request, cancellationToken)
            .ConfigureAwait(false);

        if (!validationResult.IsValid)
        {
            return Result.Invalid(validationResult.AsErrors());
        }

        var factory = new SampleCacheFactory(_factoryContext);
        var response = await factory
            .GetOrCreateAsync(request.Key, cancellationToken)
            .ConfigureAwait(false);

        return Result.Success(response);
    }
}
