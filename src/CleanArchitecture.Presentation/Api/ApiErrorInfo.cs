namespace CleanArchitecture.Presentation.Api;

/// <summary>A single error entry in an <see cref="ApiResponse"/>.</summary>
public sealed class ApiErrorInfo
{
    public ApiErrorInfo(string message)
    {
        Message = message;
    }

    /// <summary>The human-readable error message.</summary>
    public string Message { get; }
}
