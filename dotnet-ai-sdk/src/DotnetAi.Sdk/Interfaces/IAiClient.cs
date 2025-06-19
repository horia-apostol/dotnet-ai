using DotnetAi.Sdk.Models;

namespace DotnetAi.Sdk.Interfaces;

/// <summary>
/// Represents a client for interacting with an AI provider.
/// </summary>
public interface IAiClient
{
    /// <summary>
    /// Gets the name of the AI provider.
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Sends a chat request to the AI provider and retrieves the response.
    /// </summary>
    /// <param name="request">The chat request containing the prompt, messages, and other parameters.</param>
    /// <param name="apiKey">The API key used for authentication with the AI provider.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the AI's response as a string.</returns>
    Task<string> AskAsync(ChatRequest request, string apiKey);
}
