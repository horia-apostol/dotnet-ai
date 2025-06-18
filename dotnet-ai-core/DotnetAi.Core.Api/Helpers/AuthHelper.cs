using DotnetAi.Core.Api.Common;

namespace DotnetAi.Core.Api.Helpers;

public static class AuthHelper
{
    public static string? ExtractApiKey(HttpRequest request)
    {
        if (!request.Headers.TryGetValue(HeaderConstants.Authorization, out var authHeader))
            return null;

        var value = authHeader.ToString();
        if (!value.StartsWith(HeaderConstants.BearerPrefix)) return null;

        var apiKey = value[HeaderConstants.BearerPrefix.Length..];
        return string.IsNullOrWhiteSpace(apiKey) ? null : apiKey;
    }
}
