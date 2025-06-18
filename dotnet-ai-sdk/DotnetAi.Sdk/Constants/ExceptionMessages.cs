namespace DotnetAi.Sdk.Constants;

/// <summary>
/// Common exception messages used throughout the SDK.
/// </summary>
public static class ExceptionMessages
{
    /// <summary>
    /// Message indicating an empty response from the AI provider.
    /// </summary>
    public const string EmptyResponse = "Empty response from AI provider.";

    /// <summary>
    /// Message indicating a failure to parse the response from the AI provider.
    /// </summary>
    public const string JsonParseError = "Failed to parse response from AI provider.";

    /// <summary>
    /// Message indicating an unexpected error while processing the AI response.
    /// </summary>
    public const string UnexpectedError = "Unexpected error while processing AI response.";

    /// <summary>
    /// Message indicating a network error occurred while calling the AI provider.
    /// </summary>
    public const string NetworkError = "Network error occurred while calling AI provider.";

    /// <summary>
    /// Formats an HTTP error message with the provided status code, reason, and error content.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="reason">The reason phrase associated with the status code.</param>
    /// <param name="errorContent">The content of the error response.</param>
    /// <returns>A formatted HTTP error message.</returns>
    public static string HttpError(int statusCode, string? reason, string errorContent)
        => $"[{statusCode}] {reason}: {errorContent}";

    /// <summary>
    /// Message indicating that the provider must not be null or empty.
    /// </summary>
    public const string ProviderNullOrEmpty = "Provider must not be null or empty.";

    /// <summary>
    /// Formats a message indicating an unsupported provider.
    /// </summary>
    /// <param name="provider">The name of the unsupported provider.</param>
    /// <returns>A formatted message indicating the unsupported provider.</returns>
    public static string UnsupportedProvider(string provider)
        => $"Unsupported provider: {provider}";
}
