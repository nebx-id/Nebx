using System.Data.Common;

namespace Nebx.Labs.Integrations.Database.Abstractions;

/// <summary>
/// Defines a contract for opening connections to a SQL database.
/// </summary>
public interface ISqlDatabase
{
    /// <summary>
    /// Opens and returns an active database connection.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token that may be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task whose result is an open <see cref="DbConnection"/>.
    /// </returns>
    public Task<DbConnection> OpenConnection(CancellationToken cancellationToken = default);
}
