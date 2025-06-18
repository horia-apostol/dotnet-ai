using System.Net.Http.Headers;
using System.Text.Json;
using DotnetAi.Core.Api.Abstract;
using DotnetAi.Core.Api.Common;

namespace DotnetAi.Core.Api.Clients.OpenAi;

public class OpenAiClient(HttpClient httpClient) : AiClientBase(httpClient)
{
    public override string ProviderName => ProviderNames.OpenAi;
    protected override string BuildUrl() => ProviderEndpoints.OpenAi;
    protected override void PrepareRequestHeaders(HttpRequestMessage request, string apiKey)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(Common.HttpHeaders.BearerScheme, apiKey);
    }
    protected override string ExtractContent(JsonDocument doc)
    {
        return doc.RootElement
                  .GetProperty(ResponseJsonKeys.Choices)[0]
                  .GetProperty(ResponseJsonKeys.Message)
                  .GetProperty(ResponseJsonKeys.Content)
                  .GetString()
               ?? ErrorMessages.EmptyAiResponse;
    }
}
