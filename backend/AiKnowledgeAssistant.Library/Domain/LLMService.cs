using AiKnowledgeAssistant.Library.Domain.Dtos;

namespace AiKnowledgeAssistant.Library.Domain;

public class LLMService : ILLMService
{
    private readonly IOpenAiClient _client;
    private const string LLM_MODEL = "gpt-4o-mini";

    public LLMService(IOpenAiClient client)
    {
        _client = client;
    }

    public async Task<string> AskAsync(string question, string context)
    {
        var messages = new List<Message>
        {
            new (ChatRole.System, "You are a helpful assistant that answers based only on the provided context."),
            new (ChatRole.User, $"Context:\n{context}\n\nQuestion:\n{question}")
        }.ToArray();
        var request = new CompletionRequest(LLM_MODEL, 0.2f, messages);

        var response = await _client.CompleteAsync(request);
        return response is not null
            ? response.Choices.FirstOrDefault()?.Message?.Content ?? string.Empty 
            : string.Empty;
    }
}

public interface ILLMService
{
    Task<string> AskAsync(string question, string context);

}
