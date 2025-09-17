using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AiKnowledgeAssistant.Library.Domain;

public class OpenAiClient : IOpenAiClient
{
    private const string EMBED_MODEL = "text-embedding-3-small";
    private readonly HttpClient _httpClient;

    public OpenAiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<float[]> CreateEmbeddingAsync(string input)
    {
        var request = new EmbedRequestBody(input, EMBED_MODEL);
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
        var content = await response.Content.ReadAsStringAsync();
        var embedResponse = JsonSerializer.Deserialize<EmbedResponse>(content, serializerOptions);

        return embedResponse?.Data?.FirstOrDefault()?.Embedding ?? [];
    }
}

public record EmbedRequestBody(string Input, string Model);
public record EmbedResponse(List<DataPoint> Data);
public record DataPoint(string Object, int Index, float[] Embedding);

public interface IOpenAiClient
{
    Task<float[]> CreateEmbeddingAsync(string input);
}