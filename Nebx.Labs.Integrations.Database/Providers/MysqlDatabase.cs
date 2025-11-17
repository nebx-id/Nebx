using System.Data.Common;
using MySqlConnector;
using Nebx.Labs.Integrations.Database.Abstractions;

namespace Nebx.Labs.Integrations.Database.Providers;

/// <inheritdoc />
public class MySqlDatabase : ISqlDatabase
{
    private readonly string _connectionString;

    /// <summary>
    /// Creates a new <see cref="MySqlDatabase"/> with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The Mysql / MariaDB connection string.</param>
    public MySqlDatabase(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <inheritdoc />
    public async Task<DbConnection> OpenConnection(CancellationToken cancellationToken = default)
    {
        var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        return conn;
    }
}
