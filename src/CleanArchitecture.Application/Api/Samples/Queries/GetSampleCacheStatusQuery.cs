using Ardalis.Result;
using CleanArchitecture.Application.Api.Samples.Responses;
using MediatR;

namespace CleanArchitecture.Application.Api.Samples.Queries;

/// <summary>Query that returns the current contents (keys) of the sample cache.</summary>
public sealed class GetSampleCacheStatusQuery : IRequest<Result<GetSampleCacheStatusResponse>>
{
}
