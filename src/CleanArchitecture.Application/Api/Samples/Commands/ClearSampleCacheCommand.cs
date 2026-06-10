using Ardalis.Result;
using MediatR;

namespace CleanArchitecture.Application.Api.Samples.Commands;

/// <summary>Command that clears the sample cache. Returns no content on success.</summary>
public sealed class ClearSampleCacheCommand : IRequest<Result>
{
}
