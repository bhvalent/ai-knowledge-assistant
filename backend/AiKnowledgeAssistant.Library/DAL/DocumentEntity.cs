using Pgvector;

namespace AiKnowledgeAssistant.Library.DAL;

public record DocumentEntity(int Id, string Content, Vector Embedding);