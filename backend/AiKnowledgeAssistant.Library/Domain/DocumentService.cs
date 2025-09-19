using AiKnowledgeAssistant.Library.DAL;
using AiKnowledgeAssistant.Library.Domain.Dtos;

namespace AiKnowledgeAssistant.Library.Domain;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _repository;
    private readonly IEmbed _embeddingService;
    private readonly IChunk _chunkingService;

    public DocumentService(IDocumentRepository repository, IEmbed embeddingService, IChunk chunkingService)
    {
        _repository = repository;
        _embeddingService = embeddingService;
        _chunkingService = chunkingService;
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
}

public interface IDocumentService
{
    Task SaveDocumentAsync(string content, string documentName);
    Task<DocumentDto?> SearchAsync(string query);
}