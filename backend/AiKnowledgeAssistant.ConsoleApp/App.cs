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
        Console.WriteLine("Vector DB Console App");
        Console.WriteLine("Commands: save, search, exit");

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim().ToLower();

            if (input == "exit")
                break;

            if (input == "save")
            {
                Console.Write("Enter phrase to save: ");
                var phrase = Console.ReadLine();

                if (!string.IsNullOrEmpty(phrase))
                {
                    var vector = await _client.CreateEmbeddingAsync(phrase);
                    await _documentRepository.SaveAsync(new DocumentDto(phrase, vector));
                    Console.WriteLine("Saved to DB Successfully");
                }
            }
            else if (input == "search")
            {
                Console.Write("Enter phrase to search: ");
                var query = Console.ReadLine() ?? "";
                var queryVector = await _client.CreateEmbeddingAsync(query);

                var result = await _documentRepository.FindClosestAsync(queryVector);

                if (result is not null)
                    Console.WriteLine($"Best match: {result?.Content}");
                else
                    Console.WriteLine("No results found.");
            }
            else
            {
                Console.WriteLine("Unknown command. Use save, search, or exit.");
            }
        }
    }
}