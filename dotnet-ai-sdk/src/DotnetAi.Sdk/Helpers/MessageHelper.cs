using DotnetAi.Sdk.Constants;
using DotnetAi.Sdk.Models;
using System.Data;

namespace DotnetAi.Sdk.Helpers;

/// <summary>
/// Provides helper methods for building message objects for chat requests.
/// </summary>
public static class MessageHelper
{
    /// <summary>
    /// Builds an array of message objects based on the provided chat request.
    /// </summary>
    /// <param name="request">The chat request containing messages or a prompt.</param>
    /// <returns>An array of message objects with roles and content.</returns>
    public static object[] Build(ChatRequest request)
    {
        if (request.Messages is { Count: > 0 })
        {
            return [.. request.Messages
                .Select(m => new { role = m.Role, content = m.Content })
                .Cast<object>()];
        }

        return [new { role = Roles.User, content = request.Prompt }];
    }
}
