namespace AiKnowledgeAssistant.Library.Domain;

public class ChunkingService : IChunk
{
    private readonly int _maxChunkSize;
    public ChunkingService(int maxChunkSize = 100)
    {
        _maxChunkSize = maxChunkSize;
    }

    public IEnumerable<string> ChunkText(string text)
    {
        // simple split by words... Will update later
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i += _maxChunkSize)
        {
            yield return string.Join(" ", words.Skip(i).Take(_maxChunkSize));
        }
    }
}

public interface IChunk
{
    IEnumerable<string> ChunkText(string text);
}