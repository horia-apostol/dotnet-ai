using System.Net.Mime;
using System.Text;
using System.Text.Json;
using DotnetAi.Core.Api.Common;
using DotnetAi.Core.Api.Helpers;
using DotnetAi.Core.Api.Interfaces;
using DotnetAi.Core.Api.Models;

namespace DotnetAi.Core.Api.Abstract;

public abstract class AiClientBase(HttpClient httpClient) : IAiClient
{
    private readonly HttpClient _httpClient = httpClient;
    public abstract string ProviderName { get; }
    protected abstract string BuildUrl();
    protected abstract void PrepareRequestHeaders(HttpRequestMessage request, string apiKey);
    protected abstract string ExtractContent(JsonDocument doc);

    public async Task<Result<string>> AskAsync(ChatRequest request, string apiKey)
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
            MediaTypeNames.Application.Json
        );

        try
        {
            using var response = await _httpClient.SendAsync(httpRequest);
            var contentStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = await new StreamReader(contentStream).ReadToEndAsync();
                var statusCode = (int)response.StatusCode;
                return Result<string>.Failure(string.Format(ErrorMessages.ApiRequestFailed, statusCode, response.ReasonPhrase, error));
            }

            using var doc = await JsonDocument.ParseAsync(contentStream);
            var result = ExtractContent(doc);

            if (string.IsNullOrWhiteSpace(result))
                return Result<string>.Failure(ErrorMessages.EmptyAiResponse);

            return Result<string>.Success(result);
        }
        catch (HttpRequestException ex)
        {
            return Result<string>.Failure(string.Format(ErrorMessages.HttpRequestException, ex.Message));
        }
        catch (JsonException ex)
        {
            return Result<string>.Failure(string.Format(ErrorMessages.JsonParseError, ex.Message));
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(string.Format(ErrorMessages.UnexpectedError, ex.Message));
        }
    }
}
