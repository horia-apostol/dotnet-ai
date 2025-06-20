# DotnetAi.Sdk

[![NuGet](https://img.shields.io/nuget/v/DotnetAi.Sdk.svg?color=blue)](https://www.nuget.org/packages/DotnetAi.Sdk)
[![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet.svg)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](#license)

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
    Provider = "openai",
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
| `Provider`    | `string`                    | Yes      | One of `openai`, `claude`, `deepseek`                    |
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
    Provider = "openai",
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
    Provider = "openai",
    Model = "gpt-4",
    Messages = messages,
    MaxTokens = 1,
};

string response = await client.SendAsync(request);
Console.WriteLine(response); // Output: C
```

#### Using custom `HttpClient`

```csharp
var http = new HttpClient();
var client = new AiClient("openai", "your-api-key", http);
```



