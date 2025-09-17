using System.Data;

namespace AiKnowledgeAssistant.Library.DAL;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}