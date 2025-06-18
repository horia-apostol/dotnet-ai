namespace DotnetAi.Sdk.Clients.Claude;

using DotnetAi.Sdk.Constants;
using DotnetAi.Sdk.Abstract;
using System.Text.Json;

/// <summary>
/// Client for interacting with the Claude API.
/// </summary>
public class ClaudeClient(HttpClient httpClient) : AiClientBase(httpClient)
{
    /// <inheritdoc/>
    public override string ProviderName => ProviderNames.Claude;

    /// <inheritdoc/>
    protected override string BuildUrl() => ApiUrls.Claude;

    /// <inheritdoc/>
    protected override void PrepareRequestHeaders(HttpRequestMessage request, string apiKey)
    {
        request.Headers.Clear();
        request.Headers.Add(Headers.ApiKeyHeader, apiKey);
        request.Headers.Add(Headers.AnthropicVersion, AnthropicVersions.Latest);
    }

    /// <inheritdoc/>
    protected override string ExtractContent(JsonDocument doc)
    {
        return doc.RootElement
                  .GetProperty("content")[0]
                  .GetProperty("text")
                  .GetString()
               ?? "[Empty Claude response]";
    }
}
