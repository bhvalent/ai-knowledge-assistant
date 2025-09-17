using Pgvector;

namespace AiKnowledgeAssistant.Library.Domain;

public record DocumentDto(int Id, string Content, float[] Embedding)
{
    public DocumentDto(string content, float[] embedding)
        : this(0, content, embedding) { }
}