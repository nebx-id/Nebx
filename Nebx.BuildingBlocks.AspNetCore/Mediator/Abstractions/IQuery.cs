namespace Nebx.BuildingBlocks.AspNetCore.Mediator.Abstractions;

/// <summary>
/// Represents a query that returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the query result.</typeparam>
public interface IQuery<TResult> : LiteBus.Queries.Abstractions.IQuery<TResult>;