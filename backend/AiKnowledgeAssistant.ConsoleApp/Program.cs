using AiKnowledgeAssistant.Library;
using Microsoft.Extensions.Configuration;

namespace AiKnowledgeAssistant.ConsoleApp;
public class Program
{
    static async Task Main()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var apiKey = configuration["openAiKey"] ?? "";
        var openAiClient = new OpenAiClient(new HttpClient(), apiKey);

        var documents = new List<string>
        {
            "The quick brown fox jumps over the lazy dog.",
            "C# is a modern, object-oriented programming language.",
            "SQL Server is a relational database management system by Microsoft.",
            "Angular is a TypeScript-based front-end framework."
        };

        var docEmbeddings = new List<(string Text, float[] Vector)>();
        foreach (var doc in documents)
        {
            var vector = await openAiClient.CreateEmbeddingAsync(doc);
            docEmbeddings.Add((doc, vector));
        }

        Console.WriteLine("Enter a search Query: ");
        var query = Console.ReadLine() ?? "";
        var queryVector = await openAiClient.CreateEmbeddingAsync(query);

        var bestMatch = docEmbeddings
            .Select(x => new { x.Text, Score = CosineSimilarity(queryVector, x.Vector) })
            .OrderByDescending(x => x.Score)
            .FirstOrDefault();

        Console.WriteLine($"\nBest match: {bestMatch?.Text} (score {bestMatch?.Score})");
    }

    static float CosineSimilarity(float[] v1, float[] v2)
    {
        float dot = 0, mag1 = 0, mag2 = 0;
        for (int i = 0; i < v1.Length; i++)
        {
            dot += v1[i] * v2[i];
            mag1 += v1[i] * v1[i];
            mag2 += v2[i] * v2[i];
        }
        return dot / ((float)Math.Sqrt(mag1) * (float)Math.Sqrt(mag2));
    }
}
