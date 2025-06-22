# DotnetAi.Sdk

[![NuGet](https://img.shields.io/nuget/v/DotnetAi.Sdk.svg?color=blue)](https://www.nuget.org/packages/DotnetAi.Sdk)
[![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet.svg)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/horia-apostol/dotnet-ai/blob/main/dotnet-ai-sdk/LICENSE)

 Unified and extensible .NET SDK for interacting with LLMs like OpenAI, Claude (Anthropic), and DeepSeek – using a single interface.

## NuGet

Install via .NET CLI:

```bash
dotnet add package DotnetAi.Sdk
```

## Quick Start

```csharp
using DotentAi.Sdk;
using DotentAi.Sdk.Models;

AiClient client = new("openai", "sk-your-api-key");

ChatRequest request = new()
{
    Model = "gpt-4.1",
    Prompt = "What is the capital of Romania?"
};

string response = await client.SendAsync(request);
Console.WriteLine(response);
```

## [ChatRequest](https://github.com/horia-apostol/dotnet-ai/blob/main/dotnet-ai-sdk/src/DotnetAi.Sdk/Models/ChatRequest.cs) Parameters

The `ChatRequest` object defines how a chat interaction is structured. It supports both simple prompts and multi-turn messages.

| Property      | Type                        | Required | Description                                              |
| ------------- | --------------------------- | -------- | -------------------------------------------------------- |
| `Prompt`      | `string?`                   | Optional | Quick prompt (if not using messages)                     |
| `Messages`    | `List<ChatMessage>`         | Optional | Structured conversation messages                         |
| `Model`       | `string?`                   | Optional | Specific model name (`gpt-4`, `claude-3-opus`, etc.)     |
| `Temperature` | `float`<br>*(default: 0.7)* | Optional | Controls randomness. Range depends on model (see below). |
| `MaxTokens`   | `int`<br>*(default: 1000)*  | Optional | Maximum response token count                             |

## Temperature Parameter

The `temperature` parameter controls how random or creative the AI's response will be. Lower values make the output more focused and deterministic. Higher values increase variability and originality, but can sometimes reduce coherence.

| Provider     | Temperature Range |
| ------------ | ----------------- |
| **OpenAI**   | `0.0 - 2.0`       |
| **Claude**   | `0.0 - 1.0`       |
| **DeepSeek** | `0.0 - 1.5`       |

## Supported Models

- All Anthropic models: https://docs.anthropic.com/en/docs/about-claude/models/overview
- All Deepseek models: https://api-docs.deepseek.com/quick_start/pricing
- All OpenAi models: https://platform.openai.com/docs/models

## Advanced Usage

### With [ChatMessage](https://github.com/horia-apostol/dotnet-ai/blob/main/dotnet-ai-sdk/src/DotnetAi.Sdk/Models/ChatMessage.cs) history

The `ChatMessage` object defines each message within a conversation.

Each message includes:

| Property   | Type     | Description |
|------------|----------|-------------|
| `Role`     | `string` | The speaker’s role: must be `"user"` or `"assistant"` |
| `Content`  | `string` | The message text |
```csharp
List<ChatMessage> messages =
[
    new() { Role = "user", Content = "What is 5 + 3?" },
    new() { Role = "assistant", Content = "5 + 3 = 8" },
    new() { Role = "user", Content = "Add 10" }
];

string response = await client.SendAsync(new ChatRequest
{
    Model = "gpt-4.1",
    Messages = messages,
    Temperature = 0.0f,
    MaxTokens = 20
});

Console.WriteLine(response);
```

### Advanced Prompting: Putting Words in the AI's Mouth

You can guide some AI models to produce tightly constrained output, for example, forcing them to pick a single answer like `A`, `B`, or `C`, by using a crafted `assistant` message and limiting `MaxTokens`.

 This technique is inspired by [Anthropic's example](https://docs.anthropic.com/en/api/messages-examples#putting-words-in-claude%E2%80%99s-mouth) and works well with some `openai` and `claude` models. May not work with `deepseek` in current implementation.
 
#### Example

```csharp
List<ChatMessage> messages =
[
    new() { Role = "user", Content = "What is latin for Ant? (A) Apoidea, (B) Rhopalocera, (C) Formicidae" },
    new() { Role = "assistant", Content = "The answer is (" },
];

ChatRequest request = new()
{
    Model = "gpt-4",
    Messages = messages,
    MaxTokens = 1,
};

string response = await client.SendAsync(request);
Console.WriteLine(response); // Output: C
```

### Provider & Model Switching

The SDK is designed to make it easy to switch between providers and models, ensuring high availability in production environments. You can define a request chain where, if a request fails or the initial model doesn't return a satisfactory response, the application will automatically try another provider or a more advanced model. This is also useful, for example, when starting with a cheaper model and escalating to a more capable one only when needed.

```csharp
using DotnetAi.Sdk;
using DotnetAi.Sdk.Models;

AiClient claudeClient = new("claude", "anthropic-key");
AiClient deepSeekClient = new("deepseek", "deepseek-key");

List <ChatMessage> messages =
[
    new() { Role = "user", Content = "What is latin for Ant? (A) Apoidea, (B) Rhopalocera, (C) Formicidae" },
    new() { Role = "assistant", Content = "The answer is (" },
];

const int MaxTokens = 1;

ChatRequest deepseekRequest = new()
{
    Model = "deepseek-reasoner",
    Messages = messages,
    MaxTokens = MaxTokens,
};

ChatRequest claudeBadRequest = new()
{
    Model = "claude-3-sonnet-20240229",
    Messages = messages,
    MaxTokens = MaxTokens,
};
ChatRequest claudeRequest = new()
{
    Model = "claude-opus-4-20250514",
    Messages = messages,
    MaxTokens = MaxTokens,
};

string? responseContent = null;

List<Func<Task<string>>> requestChain =
[
    () => deepSeekClient.SendAsync(deepseekRequest),
    () => claudeClient.SendAsync(claudeBadRequest),
    () => claudeClient.SendAsync(claudeRequest)
];

foreach (var request in requestChain)
{
    try
    {
        responseContent = await request();
        if (responseContent != null)
            break;
    }
    catch
    {

    }
}

if (responseContent != null)
{
    Console.WriteLine(responseContent);
}
else
{
    Console.WriteLine("All attempts failed. No response available.");
}
```

### Custom LLM Client Integration

The SDK allows easy integration with any custom LLM provider by extending the [AiClientBase](https://github.com/horia-apostol/dotnet-ai/blob/main/dotnet-ai-sdk/src/DotnetAi.Sdk/Abstract/AiClientBase.cs) class. This enables you to define your own request URLs, headers, and response parsing logic.

```csharp
public class CustomClient(HttpClient httpClient) : AiClientBase(httpClient)
{
    public override string ProviderName => "custom";

    protected override string BuildUrl() => "https://api.customprovider.com/chat";

    protected override void PrepareRequestHeaders(HttpRequestMessage request, string apiKey)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(Headers.BearerScheme, apiKey);
    }

    protected override string ExtractContent(JsonDocument doc)
        => doc.RootElement
              .GetProperty("content")
              .GetString()
           ?? "[Empty response]";
}
```

Once registered, the custom client can be used like any other:

```csharp
using DotnetAi.Sdk;
using DotnetAi.Sdk.Models;

AiClient customClient = new("custom", "custom-key");

ChatRequest customRequest = new()
{
    Model = "custom-model",
    Prompt = "What's 2+2?"
};

var response = await customClient.SendAsync(customRequest);
```

This approach provides full flexibility to support proprietary or third-party LLMs with minimal effort.

### Dependency Injection

All clients in the SDK inherit from a shared base class `AiClientBase` and implement the `IAiClient` interface. This makes them easy to register and inject in ASP.NET Core.

```csharp
builder.Services.AddHttpClient<IAiClient, OpenAiClient>();
builder.Services.AddHttpClient<IAiClient, ClaudeClient>();
builder.Services.AddHttpClient<IAiClient, DeepseekClient>();
builder.Services.AddHttpClient<IAiClient, CustomClient>();
```
In this case, you don't need the `AiClient` wrapper, you can call `SendAsync` directly on the injected instance.

For a complete API example showcasing *Dependency Injection* integration and dynamic provider management, check out the [Core](https://github.com/horia-apostol/dotnet-ai/tree/main/dotnet-ai-core) project

If you don’t use *Dependency Injection*, you can instantiate the `AiClient` class directly and pass a custom `HttpClient` if needed:

```csharp
var http = new HttpClient();
var client = new AiClient("openai", "your-api-key", http);

```














