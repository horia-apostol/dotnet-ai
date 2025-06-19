# DotnetAi.Core

This project contains the original, modular implementation of an AI prompt API built with ASP.NET Core.  
It serves as the foundational reference behind [`DotnetAi.Sdk`](https://www.nuget.org/packages/DotnetAi.Sdk) and demonstrates how to integrate with multiple LLM providers using a clean and extensible architecture.

The project was initially created to explore how to structure AI interactions in a clean and testable way, using dependency injection, interface-based clients, and provider-specific logic. 
It allows easy switching between multiple AI providers like OpenAI, Claude, or DeepSeek by abstracting away the request/response handling. 
It is not intended to be reused directly as a package, but rather as an architectural reference or a playground for experimentation.

## Example usage

```http
POST /api/ai/chat
Authorization: Bearer YOUR_API_KEY
Content-Type: application/json
{
  "provider": "openai",
  "model": "gpt-4.1",
  "prompt": "What's 1+1?",
  "temperature": 0.7,
  "maxTokens": 150
}
```





