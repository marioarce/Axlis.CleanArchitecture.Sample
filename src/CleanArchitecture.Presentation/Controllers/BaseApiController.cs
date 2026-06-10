using Ardalis.Result;
using CleanArchitecture.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Presentation.Controllers;

/// <summary>
/// Base controller that dispatches requests through MediatR and converts an
/// <see cref="Ardalis.Result.Result{T}"/> into an appropriate HTTP response.
/// Derive feature controllers from this type.
/// </summary>
[ApiController]
[Route("[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger;

    protected BaseApiController(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger logger)
    {
        Mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;

        var request = _httpContextAccessor.HttpContext?.Request;
        _logger.LogInformation(
            "Init controller {ControllerType} for {Method} {Path}",
            GetType().FullName,
            request?.Method,
            request?.Path.Value);
    }

    /// <summary>
    /// Gets the MediatR mediator used to dispatch requests.
    /// </summary>
    protected IMediator Mediator { get; }

    /// <summary>
    /// Sends a request through MediatR and maps the resulting <see cref="Result{T}"/>
    /// to an <see cref="IActionResult"/> wrapped in the consistent <c>ApiResponse</c> envelope.
    /// </summary>
    protected async Task<IActionResult> SendAsync<TResponse>(IRequest<Result<TResponse>> request)
    {
        var result = await Mediator.Send(request).ConfigureAwait(false);
        return result.ToApiActionResult();
    }
}
