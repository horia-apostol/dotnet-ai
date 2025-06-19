namespace DotnetAi.Sdk.Models;

/// <summary>
/// Represents a chat message with a role and content.
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Gets or sets the role in the chat message.
    /// </summary>
    public required string Role { get; set; }

    /// <summary>
    /// Gets or sets the content of the chat message.
    /// </summary>
    public required string Content { get; set; }
}
