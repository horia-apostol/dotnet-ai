using DotnetAi.Core.Api.Common;
using DotnetAi.Core.Api.Helpers;
using DotnetAi.Core.Api.Models;
using DotnetAi.Core.Api.Providers;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAi.Core.Api.Controllers;

/// <summary>
/// Exposes endpoints for sending messages to various AI providers.
/// </summary>
[ApiController]
[Route(Routes.Ai)]
public class AiController(AiProvider provider) : ControllerBase
{
    /// <summary>
    /// Sends a chat message to the selected AI provider and returns the response.
    /// </summary>
    /// <param name="request">The chat request, including model, temperature, and messages.</param>
    /// <returns>A response from the selected AI model.</returns>
    [HttpPost("chat")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        var apiKey = AuthHelper.ExtractApiKey(Request);
        if (apiKey is null)
            return Unauthorized(new { error = ErrorMessages.MissingAuthHeader });

        var providerResult = provider.GetClient(request.Provider);
        if (!providerResult.IsSuccess)
            return BadRequest(new { error = providerResult.Error });

        var client = providerResult.Value;
        if (client is null)
            return BadRequest(new { error = ErrorMessages.MissingAiClient });

        var response = await client.AskAsync(request, apiKey);

        if (!response.IsSuccess)
            return BadRequest(new { error = response.Error });

        return Ok(new { response = response.Value });
    }
}
