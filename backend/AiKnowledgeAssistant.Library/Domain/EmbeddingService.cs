using AiKnowledgeAssistant.Library.Domain.Dtos;

namespace AiKnowledgeAssistant.Library.Domain;

public class EmbeddingService : IEmbed
{
    private readonly IOpenAiClient _client;
    private const string EMBED_MODEL = "text-embedding-3-small";

    public EmbeddingService(IOpenAiClient client)
    {
        _client = client;
    }

    public async Task<float[]> GetEmbeddingAsync(string input)
    {
        var request = new EmbedRequestBody(input, EMBED_MODEL);
        var response = await _client.CreateEmbeddingAsync(request);

        return response?.Data?.FirstOrDefault()?.Embedding ?? [];
    }
}

public interface IEmbed
{
    Task<float[]> GetEmbeddingAsync(string input);
}