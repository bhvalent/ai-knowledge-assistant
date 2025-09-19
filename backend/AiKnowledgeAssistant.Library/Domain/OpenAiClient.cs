using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AiKnowledgeAssistant.Library.Domain.Dtos;

namespace AiKnowledgeAssistant.Library.Domain;

public class OpenAiClient : IOpenAiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public OpenAiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        _serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    public async Task<EmbedResponse?> CreateEmbeddingAsync(EmbedRequest request)
    {
        var response = await _httpClient.PostAsync(
            "v1/embeddings",
            new StringContent(
                JsonSerializer.Serialize(request, _serializerOptions),
                Encoding.UTF8,
                "application/json"));
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmbedResponse>(content, _serializerOptions);
    }

    public async Task<CompletionResponse?> CompleteAsync(CompletionRequest request)
    {
        var response = await _httpClient.PostAsync(
            "v1/chat/completions",
            new StringContent(
                JsonSerializer.Serialize(request, _serializerOptions),
                Encoding.UTF8,
                "application/json"));
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompletionResponse>(content, _serializerOptions);
    }
}

public interface IOpenAiClient
{
    Task<EmbedResponse?> CreateEmbeddingAsync(EmbedRequest request);
    Task<CompletionResponse?> CompleteAsync(CompletionRequest request);
}