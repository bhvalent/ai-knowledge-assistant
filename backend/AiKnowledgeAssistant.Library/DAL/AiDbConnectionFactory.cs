using System.Data;
using Npgsql;

namespace AiKnowledgeAssistant.Library.DAL;

public class AiDbConnectionFactory : IDbConnectionFactory, IDisposable
{
    private readonly NpgsqlDataSource _dataSource;

    public AiDbConnectionFactory(string connectionString)
    {
        var dsb = new NpgsqlDataSourceBuilder(connectionString);
        dsb.UseVector();
        _dataSource = dsb.Build();
    }

    public async Task<IDbConnection> CreateConnectionAsync()
        => await _dataSource.OpenConnectionAsync();

    public void Dispose()
    {
        _dataSource.Dispose();
    }
}
