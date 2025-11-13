using System.Data.Common;
using Nebx.Labs.Integrations.Database.Abstractions;
using Npgsql;

namespace Nebx.Labs.Integrations.Database.Providers;

/// <summary>
/// Provides an <see cref="ISqlDatabase"/> implementation for PostgreSQL.
/// </summary>
public class PostgresDatabase : ISqlDatabase
{
    private readonly string _connectionString;

    /// <summary>
    /// Creates a new <see cref="PostgresDatabase"/> with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The PostgreSQL connection string.</param>
    public PostgresDatabase(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <inheritdoc />
    public async Task<DbConnection> OpenConnection(CancellationToken cancellationToken = default)
    {
        var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        return conn;
    }
}
