using DotnetAi.Functions.Constants;
using DotnetAi.Functions.Models;
using DotnetAi.Sdk;
using DotnetAi.Sdk.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace DotnetAi.Functions;

public class ChatFunction
{
    private readonly AiClient _aiClient;
    public ChatFunction()
    {
        var apiKey = Environment.GetEnvironmentVariable("OpenAiKey");
        _aiClient = new AiClient(ProviderNames.OpenAi, apiKey!);
    }

    [Function("chat")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        var body = await req.ReadFromJsonAsync<PromptRequest>();

        var chatRequest = new ChatRequest
        {
            Model = AiModels.Gpt4,
            Prompt = body!.Prompt,
            Temperature = body.Temperature,
            MaxTokens = body.MaxTokens
        };

        var result = req.CreateResponse(HttpStatusCode.OK);
        await result.WriteAsJsonAsync(
            new
            {
                response = await _aiClient.SendAsync(chatRequest) 
            });

        return result;
    }
}
