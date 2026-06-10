using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Presentation.Api;

/// <summary>Generic <see cref="ApiResponse"/> that carries a typed payload.</summary>
/// <typeparam name="T">The payload type.</typeparam>
public sealed class ApiResponse<T> : ApiResponse
{
    /// <summary>The response payload.</summary>
    public T? Data { get; init; }

    /// <summary>Creates a 200 OK envelope wrapping <paramref name="data"/>.</summary>
    public static ApiResponse<T> Ok(T? data, string? message = null)
        => new() { Success = true, StatusCode = StatusCodes.Status200OK, Message = message, Data = data };
}
