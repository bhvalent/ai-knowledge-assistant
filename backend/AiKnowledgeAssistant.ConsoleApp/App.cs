using AiKnowledgeAssistant.Library.Domain;

namespace AiKnowledgeAssistant.ConsoleApp;

public class App
{
    private readonly IDocumentService _documentService;

    public App(IDocumentService documentService)
    {
        _documentService = documentService;
    }
    
    public async Task Run()
    {
        Console.WriteLine("Vector DB Console App");

        while (true)
        {
            Console.WriteLine("Commands: save, search, exit");
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim().ToLower();

            if (input == "exit")
                break;

            if (input == "save")
            {
                Console.Write("Enter document name: ");
                var docName = Console.ReadLine() ?? "doc";

                Console.Write("Enter content: ");
                var content = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrEmpty(content))
                {
                    await _documentService.SaveDocumentAsync(content, docName);
                    Console.WriteLine("Saved to DB Successfully");
                }
            }
            else if (input == "search")
            {
                Console.Write("Enter phrase to search: ");
                var query = Console.ReadLine() ?? "";
                var result = await _documentService.SearchAsync(query);

                if (result is not null)
                    Console.WriteLine($"Found chunk: {result.Content} (metadata: {result.Metadata})");
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