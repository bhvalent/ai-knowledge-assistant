using AiKnowledgeAssistant.Library.DAL;
using AiKnowledgeAssistant.Library.Domain.Dtos;

namespace AiKnowledgeAssistant.Library.Domain;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _repository;
    private readonly IEmbed _embeddingService;
    private readonly IChunk _chunkingService;
    private readonly ILLMService _llmService;

    public DocumentService(
        IDocumentRepository repository,
        IEmbed embeddingService,
        IChunk chunkingService,
        ILLMService llmService)
    {
        _repository = repository;
        _embeddingService = embeddingService;
        _chunkingService = chunkingService;
        _llmService = llmService;
    }

    public async Task SaveDocumentAsync(string content, string documentName)
    {
        var chunks = _chunkingService.ChunkText(content).ToList();

        int chunkNumber = 1;
        foreach (var chunk in chunks)
        {
            var embedding = await _embeddingService.GetEmbeddingAsync(chunk);
            string metadata = $"{documentName}-chunk-{chunkNumber}";
            await _repository.SaveChunkAsync(new DocumentDto(chunk, embedding, metadata));
            chunkNumber++;
        }
    }

    public async Task<DocumentDto?> SearchAsync(string query)
    {
        var embedding = await _embeddingService.GetEmbeddingAsync(query);
        return await _repository.FindClosestAsync(embedding);
    }

    public async Task<string> AskAsync(string question)
    {
        var embedding = await _embeddingService.GetEmbeddingAsync(question);
        var result = await _repository.FindClosestAsync(embedding);

        if (result == default)
            return "No relevant context found.";

        return await _llmService.AskAsync(question, result.Content);
    }
}

public interface IDocumentService
{
    Task SaveDocumentAsync(string content, string documentName);
    Task<DocumentDto?> SearchAsync(string query);
    Task<string> AskAsync(string question);
}