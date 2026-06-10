using Ardalis.Result;
using CleanArchitecture.Presentation.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Extensions;

/// <summary>
/// Maps an Ardalis <see cref="Result{T}"/> to an <see cref="IActionResult"/> using the
/// consistent <see cref="ApiResponse"/> envelope.
/// </summary>
internal static class ResultExtensions
{
    public static IActionResult ToApiActionResult<T>(this Result<T> result)
    {
        switch (result.Status)
        {
            case ResultStatus.Ok:
                return new OkObjectResult(ApiResponse<T>.Ok(result.Value, result.SuccessMessage));

            case ResultStatus.Invalid:
                var validationErrors = result.ValidationErrors
                    .Select(error => new ApiErrorInfo(error.ErrorMessage))
                    .ToList();
                return new BadRequestObjectResult(ApiResponse.BadRequest(validationErrors));

            case ResultStatus.NotFound:
                return new NotFoundObjectResult(ApiResponse.NotFound(ToErrors(result.Errors)));

            case ResultStatus.Unauthorized:
                return new UnauthorizedObjectResult(ApiResponse.Unauthorized(ToErrors(result.Errors)));

            case ResultStatus.Forbidden:
                return new ObjectResult(ApiResponse.Forbidden(ToErrors(result.Errors)))
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                };

            default:
                return new BadRequestObjectResult(ApiResponse.BadRequest(ToErrors(result.Errors)));
        }
    }

    private static List<ApiErrorInfo> ToErrors(IEnumerable<string> errors)
        => errors
            .Where(error => !string.IsNullOrEmpty(error))
            .Select(error => new ApiErrorInfo(error))
            .ToList();
}
