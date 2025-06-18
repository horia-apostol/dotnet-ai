using DotnetAi.Core.Api.Interfaces;
using DotnetAi.Core.Api.Common;

namespace DotnetAi.Core.Api.Providers;

public class AiProvider(IEnumerable<IAiClient> clients)
{
    private readonly Dictionary<string, IAiClient> _clients =
        clients.ToDictionary(c => c.ProviderName.ToLowerInvariant());

    public Result<IAiClient> GetClient(string provider)
    {
        if (string.IsNullOrWhiteSpace(provider))
            return Result<IAiClient>.Failure(ErrorMessages.MissingProvider);

        var key = provider.ToLowerInvariant();

        if (_clients.TryGetValue(key, out var client))
            return Result<IAiClient>.Success(client);

        return Result<IAiClient>.Failure(string.Format(ErrorMessages.UnsupportedProvider, provider));
    }
}
