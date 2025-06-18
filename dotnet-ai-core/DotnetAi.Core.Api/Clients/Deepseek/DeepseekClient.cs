using System.Net.Http.Headers;
using System.Text.Json;
using DotnetAi.Core.Api.Abstract;
using DotnetAi.Core.Api.Common;

namespace DotnetAi.Core.Api.Clients.Deepseek;

public class DeepseekClient(HttpClient httpClient) : AiClientBase(httpClient)
{
    public override string ProviderName => ProviderNames.Deepseek;
    protected override string BuildUrl() => ProviderEndpoints.Deepseek;
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
