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
        if (result.Status == ResultStatus.Ok)
        {
            return new OkObjectResult(ApiResponse<T>.Ok(result.Value, result.SuccessMessage));
        }

        return MapNonOk(result.Status, result.Errors, result.ValidationErrors);
    }

    public static IActionResult ToApiActionResult(this Result result)
    {
        if (result.Status == ResultStatus.Ok)
        {
            return new OkObjectResult(ApiResponse.Ok(result.SuccessMessage));
        }

        return MapNonOk(result.Status, result.Errors, result.ValidationErrors);
    }

    private static IActionResult MapNonOk(
        ResultStatus status,
        IEnumerable<string> errors,
        IEnumerable<ValidationError> validationErrors)
    {
        switch (status)
        {
            case ResultStatus.NoContent:
                return new NoContentResult();

            case ResultStatus.Invalid:
                var ve = validationErrors
                    .Select(error => new ApiErrorInfo(error.ErrorMessage))
                    .ToList();
                return new BadRequestObjectResult(ApiResponse.BadRequest(ve));

            case ResultStatus.NotFound:
                return new NotFoundObjectResult(ApiResponse.NotFound(ToErrors(errors)));

            case ResultStatus.Unauthorized:
                return new UnauthorizedObjectResult(ApiResponse.Unauthorized(ToErrors(errors)));

            case ResultStatus.Forbidden:
                return new ObjectResult(ApiResponse.Forbidden(ToErrors(errors)))
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                };

            default:
                return new BadRequestObjectResult(ApiResponse.BadRequest(ToErrors(errors)));
        }
    }

    private static List<ApiErrorInfo> ToErrors(IEnumerable<string> errors)
        => errors
            .Where(error => !string.IsNullOrEmpty(error))
            .Select(error => new ApiErrorInfo(error))
            .ToList();
}
