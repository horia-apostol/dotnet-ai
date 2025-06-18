namespace DotnetAi.Sdk.Clients.Deepseek;

using DotnetAi.Sdk.Constants;
using DotnetAi.Sdk.Abstract;
using System.Net.Http.Headers;
using System.Text.Json;

/// <summary>
/// Client for interacting with the DeepSeek API.
/// </summary>
public class DeepseekClient(HttpClient httpClient) : AiClientBase(httpClient)
{
    /// <inheritdoc/>
    public override string ProviderName => ProviderNames.Deepseek;

    /// <inheritdoc/>
    protected override string BuildUrl() => ApiUrls.Deepseek;

    /// <inheritdoc/>
    protected override void PrepareRequestHeaders(HttpRequestMessage request, string apiKey)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(Headers.BearerScheme, apiKey);
    }

    /// <inheritdoc/>
    protected override string ExtractContent(JsonDocument doc)
    {
        return doc.RootElement
                  .GetProperty("choices")[0]
                  .GetProperty("message")
                  .GetProperty("content")
                  .GetString()
               ?? "[Empty DeepSeek response]";
    }
}
