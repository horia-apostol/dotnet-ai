# DotnetAi.Functions

This project contains an Azure Function example that exposes a simple HTTP endpoint for interacting with large language models via [DotnetAi.Sdk](https://www.nuget.org/packages/DotnetAi.Sdk).

## Description

The `chat` function accepts a `POST` request with a prompt, temperature, and max tokens. It uses the `DotnetAi.Sdk` to send the request to OpenAI's GPT-4 model and returns the raw response.

### Request format

```http
POST /api/chat
Content-Type: application/json

{
  "prompt": "Tell me a joke.",
  "temperature": 0.7,
  "maxTokens": 100
}
```

### Response format

```json
{
    "response": "Why don't scientists trust atoms?\n\nBecause they make up everything!"
}
```

