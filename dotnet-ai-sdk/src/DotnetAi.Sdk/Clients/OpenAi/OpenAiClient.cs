
using System.Net.Http.Headers;
using System.Text.Json;
using DotnetAi.Sdk.Constants;
using DotnetAi.Sdk.Abstract;

namespace DotnetAi.Sdk.Clients.OpenAi;
/// <summary>
/// Represents a client for interacting with the OpenAI API.
/// </summary>
public class OpenAiClient(HttpClient httpClient) : AiClientBase(httpClient)
{

    /// <inheritdoc/>
    public override string ProviderName => ProviderNames.OpenAi;

    /// <inheritdoc/>
    protected override string BuildUrl() => ApiUrls.OpenAi;

    /// <inheritdoc/>
    protected override void PrepareRequestHeaders(HttpRequestMessage request, string apiKey)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(Headers.BearerScheme, apiKey);
    }

    /// <inheritdoc/>
    protected override string ExtractContent(JsonDocument doc)
        => doc.RootElement
              .GetProperty("choices")[0]
              .GetProperty("message")
              .GetProperty("content")
              .GetString()
           ?? "[Empty OpenAI response]";
}
