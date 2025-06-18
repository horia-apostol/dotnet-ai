# DotnetAi.Sdk

[![NuGet](https://img.shields.io/nuget/v/DotnetAi.Sdk.svg?color=blue)](https://www.nuget.org/packages/DotnetAi.Sdk)
[![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet.svg)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](#license)

> Unified and extensible .NET SDK for interacting with LLMs like OpenAI, Claude (Anthropic), and DeepSeek – using a single interface.

## NuGet

Install via .NET CLI:

```bash
dotnet add package DotnetAi.Sdk
```

## Quick Start

```csharp
using DotentAi.Sdk;
using DotentAi.Sdk.Models;

var client = new AiClient("openai", "sk-your-api-key");

var request = new ChatRequest
{
    Provider = "openai",
    Model = "gpt-3.5-turbo",
    Messages = new()
    {
        new() { Role = "user", Content = "What's the capital of France?" }
    }
};

var response = await client.SendAsync(request);
Console.WriteLine(response);
```

## Configuration

| Provider     | ID         | Default Model            | Temperature Range |
|--------------|------------|--------------------------|-------------------|
| **OpenAI**   | `openai`   | `gpt-3.5-turbo`          | `0.0 - 2.0`       |
| **Claude**   | `claude`   | `claude-3-opus-20240229` | `0.0 - 1.0`       |
| **DeepSeek** | `deepseek` | `deepseek-chat`          | `0.0 - 1.5`       |
