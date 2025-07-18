using System.Data.Common;

namespace Nebx.BuildingBlocks.AspNetCore.Data;

/// <summary>
///     Defines a contract for opening connections to a SQL database.
/// </summary>
public interface ISqlDatabase
{
    /// <summary>
    ///     Opens and returns an active database connection.
    /// </summary>
    /// <param name="ct">An optional <see cref="CancellationToken" /> that can be used to cancel the operation.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains an open <see cref="DbConnection" />.
    /// </returns>
    public Task<DbConnection> OpenConnection(CancellationToken ct = default);
}