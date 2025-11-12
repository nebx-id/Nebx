namespace Nebx.BuildingBlocks.AspNetCore.Mediator.Abstractions;

/// <summary>
/// Handles a specific query and returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TResult">The result type.</typeparam>
public interface IQueryHandler<in TQuery, TResult>
    : LiteBus.Queries.Abstractions.IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>;