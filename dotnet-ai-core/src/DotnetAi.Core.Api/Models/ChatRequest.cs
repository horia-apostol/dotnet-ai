namespace DotnetAi.Core.Api.Models;

public class ChatRequest
{
    public required string Provider { get; set; }
    public string? Prompt { get; set; }
    public List<ChatMessage>? Messages { get; set; }
    public string? Model { get; set; }
    public float Temperature { get; set; } = 0.7f;
    public int MaxTokens { get; set; } = 1000;
}