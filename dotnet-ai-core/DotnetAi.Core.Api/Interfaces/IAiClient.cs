using DotnetAi.Core.Api.Common;
using DotnetAi.Core.Api.Models;

namespace DotnetAi.Core.Api.Interfaces;

public interface IAiClient
{
    string ProviderName { get; }
    Task<Result<string>> AskAsync(ChatRequest request, string apiKey);
}
