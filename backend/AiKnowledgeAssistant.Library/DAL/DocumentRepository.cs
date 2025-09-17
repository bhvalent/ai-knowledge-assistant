using AiKnowledgeAssistant.Library.Domain;
using Dapper;
using Pgvector;

namespace AiKnowledgeAssistant.Library.DAL;

public class DocumentRepository : IDocumentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DocumentRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task SaveAsync(DocumentDto document)
    {
        var entity = DocumentMapper.ToPersistance(document);

        using var conn = await _connectionFactory.CreateConnectionAsync();
        await conn.ExecuteAsync(
            """
            INSERT INTO documents
                (content, embedding)
                VALUES(@Content, @Embedding);
            """,
            entity
        );
    }

    public async Task<DocumentDto?> FindClosestAsync(float[] queryEmbedding)
    {
        using var conn = await _connectionFactory.CreateConnectionAsync();
        var vector = new Vector(queryEmbedding);
        var result = await conn.QueryFirstOrDefaultAsync<DocumentEntity>(
            """
                SELECT
                    id, content, embedding
                FROM documents
                ORDER BY embedding <-> @queryEmbedding
                LIMIT 1;
            """,
            new { queryEmbedding = vector }
        );

        return result is not null ? DocumentMapper.ToDomain(result) : null;
    }
}

public interface IDocumentRepository
{
    Task SaveAsync(DocumentDto document);
    Task<DocumentDto?> FindClosestAsync(float[] query);
}
