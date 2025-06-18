using System.Net.Mime;
using System.Text;
using System.Text.Json;
using DotnetAi.Sdk.Constants;
using DotnetAi.Sdk.Exceptions;
using DotnetAi.Sdk.Helpers;
using DotnetAi.Sdk.Interfaces;
using DotnetAi.Sdk.Models;

namespace DotnetAi.Sdk.Abstract;

/// <summary>
/// Base class for AI clients, handling HTTP logic and error handling.
/// </summary>
public abstract class AiClientBase(HttpClient httpClient) : IAiClient
{
    private readonly HttpClient _httpClient = httpClient;

    /// <inheritdoc/>
    public abstract string ProviderName { get; }

    /// <summary>
    /// Builds the URL for the AI provider's API endpoint.
    /// </summary>
    protected abstract string BuildUrl();

    /// <summary>
    /// Prepares the HTTP request headers with necessary information.
    /// </summary>
    protected abstract void PrepareRequestHeaders(HttpRequestMessage request, string apiKey);

    /// <summary>
    /// Extracts the relevant content from the JSON document returned by the AI provider.
    /// </summary>
    protected abstract string ExtractContent(JsonDocument doc);

    /// <inheritdoc/>
    public async Task<string> AskAsync(ChatRequest request, string apiKey)
    {
        var payload = new
        {
            model = request.Model,
            temperature = request.Temperature,
            max_tokens = request.MaxTokens,
            messages = MessageHelper.Build(request)
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, BuildUrl());
        PrepareRequestHeaders(httpRequest, apiKey);

        httpRequest.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        try
        {
            using var response = await _httpClient.SendAsync(httpRequest);
            var stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await new StreamReader(stream).ReadToEndAsync();
                var statusCode = (int)response.StatusCode;
                throw new AiClientException(ExceptionMessages.HttpError(statusCode, response.ReasonPhrase, errorText));
            }

            using var doc = await JsonDocument.ParseAsync(stream);
            var result = ExtractContent(doc);

            if (string.IsNullOrWhiteSpace(result))
                throw new AiClientException(ExceptionMessages.EmptyResponse);

            return result;
        }
        catch (HttpRequestException ex)
        {
            throw new AiClientException(ExceptionMessages.NetworkError, ex);
        }
        catch (JsonException ex)
        {
            throw new AiClientException(ExceptionMessages.JsonParseError, ex);
        }
        catch (Exception ex)
        {
            throw new AiClientException(ExceptionMessages.UnexpectedError, ex);
        }
    }
}
