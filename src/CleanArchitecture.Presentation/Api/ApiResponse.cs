using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Presentation.Api;

/// <summary>
/// Minimal, consistent response envelope returned by every endpoint. Carries success state,
/// the HTTP status code, an optional message, and any errors.
/// </summary>
public class ApiResponse
{
    /// <summary>Whether the request succeeded.</summary>
    public bool Success { get; init; }

    /// <summary>The HTTP status code associated with the response.</summary>
    public int StatusCode { get; init; }

    /// <summary>An optional human-readable message.</summary>
    public string? Message { get; init; }

    /// <summary>Any errors associated with a non-success response.</summary>
    public IReadOnlyList<ApiErrorInfo> Errors { get; init; } = Array.Empty<ApiErrorInfo>();

    /// <summary>Creates a 400 Bad Request envelope.</summary>
    public static ApiResponse BadRequest(IReadOnlyList<ApiErrorInfo> errors)
        => new() { Success = false, StatusCode = StatusCodes.Status400BadRequest, Errors = errors };

    /// <summary>Creates a 404 Not Found envelope.</summary>
    public static ApiResponse NotFound(IReadOnlyList<ApiErrorInfo> errors)
        => new() { Success = false, StatusCode = StatusCodes.Status404NotFound, Errors = errors };

    /// <summary>Creates a 401 Unauthorized envelope.</summary>
    public static ApiResponse Unauthorized(IReadOnlyList<ApiErrorInfo> errors)
        => new() { Success = false, StatusCode = StatusCodes.Status401Unauthorized, Errors = errors };

    /// <summary>Creates a 403 Forbidden envelope.</summary>
    public static ApiResponse Forbidden(IReadOnlyList<ApiErrorInfo> errors)
        => new() { Success = false, StatusCode = StatusCodes.Status403Forbidden, Errors = errors };
}
