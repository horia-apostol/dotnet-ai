namespace DotnetAi.Sdk.Constants;

/// <summary>
/// Contains constants for HTTP headers used in the SDK.
/// </summary>
public static class Headers
{
    /// <summary>
    /// The Bearer authentication scheme.
    /// </summary>
    public const string BearerScheme = "Bearer";

    /// <summary>
    /// The header name for the API key.
    /// </summary>
    public const string ApiKeyHeader = "x-api-key";

    /// <summary>
    /// The header name for specifying the Anthropic version.
    /// </summary>
    public const string AnthropicVersion = "anthropic-version";
}
