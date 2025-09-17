using Dapper;
using Pgvector.Dapper;

namespace AiKnowledgeAssistant.Library.DAL;

public static class DapperConfig
{
    public static void RegisterVectorMapping()
    {
        SqlMapper.AddTypeHandler(new VectorTypeHandler());
    }
}