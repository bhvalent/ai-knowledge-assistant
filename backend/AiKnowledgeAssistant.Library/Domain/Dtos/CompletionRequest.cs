namespace AiKnowledgeAssistant.Library.Domain.Dtos;

public record CompletionRequest(string Model, float Temperature, Message[] Messages);