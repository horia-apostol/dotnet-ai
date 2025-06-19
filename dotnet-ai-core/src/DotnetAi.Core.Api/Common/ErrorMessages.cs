namespace DotnetAi.Core.Api.Common;

public static class ErrorMessages
{
    public const string MissingAuthHeader = "Missing or invalid Authorization header";
    public const string EmptyApiKey = "Empty API key";
    public const string MissingAiClient = "Internal error: AI client is missing";
    public const string MissingProvider = "Provider is missing.";
    public const string UnsupportedProvider = "Unsupported AI provider: '{0}'";

    public const string ApiRequestFailed = "[{0}] {1}: {2}";
    public const string EmptyAiResponse = "The AI provider returned an empty response.";
    public const string HttpRequestException = "HTTP error occurred: {0}";
    public const string JsonParseError = "Failed to parse AI response: {0}";
    public const string UnexpectedError = "Unexpected error: {0}";
}
