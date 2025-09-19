namespace AiKnowledgeAssistant.Library.Domain.Dtos;

public record DocumentDto(int Id, string Content, float[] Embedding, string Metadata)
{
    public DocumentDto(string content, float[] embedding, string metadata)
        : this(0, content, embedding, metadata) { }
}