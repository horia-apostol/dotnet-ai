namespace DotnetAi.Sdk.Models;

/// <summary>
/// Represents a request to a chat provider with optional prompt, messages, and configuration settings.
/// </summary>
public class ChatRequest
{
    /// <summary>
    /// Gets or sets the prompt for the chat request.
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    /// Gets or sets the list of chat messages for the request.
    /// </summary>
    public List<ChatMessage>? Messages { get; set; }

    /// <summary>
    /// Gets or sets the model to be used for the chat request.
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Gets or sets the temperature value for the chat request, which controls randomness.
    /// </summary>
    public float Temperature { get; set; } = 0.7f;

    /// <summary>
    /// Gets or sets the maximum number of tokens for the chat response.
    /// </summary>
    public int MaxTokens { get; set; } = 1000;
}
