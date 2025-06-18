using System.Text.Json;
using DotnetAi.Core.Api.Abstract;
using DotnetAi.Core.Api.Common;

namespace DotnetAi.Core.Api.Clients.Claude;

public class ClaudeClient(HttpClient httpClient) : AiClientBase(httpClient)
{
    public override string ProviderName => ProviderNames.Claude;
    protected override string BuildUrl() => ProviderEndpoints.Claude;
    protected override void PrepareRequestHeaders(HttpRequestMessage request, string apiKey)
    {
        request.Headers.Clear();
        request.Headers.Add(HttpHeaders.ApiKeyHeader, apiKey);
        request.Headers.Add(HttpHeaders.AnthropicVersion, Versions.AnthropicApiVersion);
    }

    protected override string ExtractContent(JsonDocument doc)
    {
        return doc.RootElement
                  .GetProperty(ResponseJsonKeys.Content)[0]
                  .GetProperty(ResponseJsonKeys.Text)
                  .GetString()
               ?? ErrorMessages.EmptyAiResponse;
    }
}
