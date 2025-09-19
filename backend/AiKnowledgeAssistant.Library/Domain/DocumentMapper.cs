using AiKnowledgeAssistant.Library.DAL;
using AiKnowledgeAssistant.Library.Domain.Dtos;
using Pgvector;

namespace AiKnowledgeAssistant.Library.Domain;

public static class DocumentMapper
{
    public static DocumentDto ToDomain(DocumentEntity entity) => new(entity.Id, entity.Content, entity.Embedding.ToArray(), entity.Metadata);
    public static DocumentEntity ToPersistance(DocumentDto dto) => new(dto.Id, dto.Content, new Vector(dto.Embedding), dto.Metadata);
}