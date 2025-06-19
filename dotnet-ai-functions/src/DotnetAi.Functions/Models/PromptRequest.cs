namespace DotnetAi.Functions.Models;

public class PromptRequest
{
    public string Prompt { get; set; } = string.Empty;
    public float Temperature { get; set; } = 0.7f;
    public int MaxTokens { get; set; } = 200;
}
