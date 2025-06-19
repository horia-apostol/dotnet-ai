using DotnetAi.Core.Api.Common;
using DotnetAi.Core.Api.Models;

namespace DotnetAi.Core.Api.Helpers;

public static class MessageHelper
{
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
