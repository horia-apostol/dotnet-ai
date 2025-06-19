using DotnetAi.Sdk.Clients.Claude;
using DotnetAi.Sdk.Clients.Deepseek;
using DotnetAi.Sdk.Clients.OpenAi;
using DotnetAi.Sdk.Constants;
using DotnetAi.Sdk.Interfaces;
using DotnetAi.Sdk.Models;

namespace DotnetAi.Sdk;

/// <summary>
/// Unified AI client for interacting with different providers.
/// </summary>
public class AiClient
{
    private readonly IAiClient _client;
    private readonly string _apiKey;

    /// <summary>
    /// Simple constructor – internally manages the HttpClient instance.
    /// </summary>
    public AiClient(string provider, string apiKey)
        : this(provider, apiKey, new HttpClient())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AiClient"/> class.
    /// </summary>
    /// <param name="provider">The name of the AI provider (e.g., OpenAI, Claude, Deepseek).</param>
    /// <param name="apiKey">The API key for authenticating with the provider.</param>
    /// <param name="httpClient">The HTTP client used for making requests.</param>
    /// <exception cref="ArgumentException">Thrown when the provider is null, empty, or unsupported.</exception>
    public AiClient(string provider, string apiKey, HttpClient httpClient)
    {
        if (string.IsNullOrWhiteSpace(provider))
            throw new ArgumentException(ExceptionMessages.ProviderNullOrEmpty, nameof(provider));

        _client = provider.ToLowerInvariant() switch
        {
            ProviderNames.OpenAi => new OpenAiClient(httpClient),
            ProviderNames.Claude => new ClaudeClient(httpClient),
            ProviderNames.Deepseek => new DeepseekClient(httpClient),
            _ => throw new ArgumentException(ExceptionMessages.UnsupportedProvider(provider), nameof(provider))
        };

        _apiKey = apiKey;
    }

    /// <summary>
    /// Sends a chat request to the configured AI provider.
    /// </summary>
    /// <param name="request">The chat request.</param>
    /// <returns>The AI response or an error.</returns>
    public Task<string> SendAsync(ChatRequest request)
        => _client.AskAsync(request, _apiKey);
}
