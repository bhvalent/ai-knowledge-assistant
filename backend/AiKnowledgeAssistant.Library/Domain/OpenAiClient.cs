using System.Text;
using System.Text.Json;
using AiKnowledgeAssistant.Library.Domain.Dtos;

namespace AiKnowledgeAssistant.Library.Domain;

public class OpenAiClient : IOpenAiClient
{
    private readonly HttpClient _httpClient;

    public OpenAiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EmbedResponse?> CreateEmbeddingAsync(EmbedRequestBody request)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
        var response = await _httpClient.PostAsync(
            "v1/embeddings",
            new StringContent(
                JsonSerializer.Serialize(request, serializerOptions),
                Encoding.UTF8,
                "application/json"));
        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EmbedResponse>(content, serializerOptions);
    }
}

public interface IOpenAiClient
{
    Task<EmbedResponse?> CreateEmbeddingAsync(EmbedRequestBody request);
}