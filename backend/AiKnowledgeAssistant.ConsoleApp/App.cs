using AiKnowledgeAssistant.Library.DAL;
using AiKnowledgeAssistant.Library.Domain;
using Pgvector;

namespace AiKnowledgeAssistant.ConsoleApp;

public class App
{
    private readonly IOpenAiClient _client;
    private readonly IDocumentRepository _documentRepository;

    public App(IOpenAiClient openAiClient, IDocumentRepository documentRepository)
    {
        _client = openAiClient;
        _documentRepository = documentRepository;
    }
    
    public async Task Run()
    {
        // var documents = new List<string>
        // {
        //     "The quick brown fox jumps over the lazy dog.",
        //     "C# is a modern, object-oriented programming language.",
        //     "SQL Server is a relational database management system by Microsoft.",
        //     "Angular is a TypeScript-based front-end framework."
        // };

        // foreach (var doc in documents)
        // {
        //     var vector = await _client.CreateEmbeddingAsync(doc);
        //     await _documentRepository.SaveAsync(new DocumentDto(doc, vector));
        // }

        Console.WriteLine("Enter a search Query: ");
        var query = Console.ReadLine() ?? "";
        var queryVector = await _client.CreateEmbeddingAsync(query);

        var bestMatch = await _documentRepository.FindClosestAsync(queryVector);

        Console.WriteLine($"\nBest match: {bestMatch?.Content} (id: {bestMatch?.Id})");
    }
}