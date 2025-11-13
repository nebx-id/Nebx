using System.Data.Common;
using Microsoft.Data.SqlClient;
using Nebx.Labs.Integrations.Database.Abstractions;

namespace Nebx.Labs.Integrations.Database.Providers;

/// <summary>
/// Provides an <see cref="ISqlDatabase"/> implementation for SQL Server.
/// </summary>
public class SqlServerDatabase : ISqlDatabase
{
    private readonly string _connectionString;

    /// <summary>
    /// Creates a new <see cref="SqlServerDatabase"/> with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The SQL Server connection string.</param>
    public SqlServerDatabase(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <inheritdoc />
    public async Task<DbConnection> OpenConnection(CancellationToken cancellationToken = default)
    {
        var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);
        return conn;
    }
}
