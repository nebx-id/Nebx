using Nebx.BuildingBlocks.AspNetCore.Contracts.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.Mediator.Abstractions;

/// <summary>
/// Defines a mediator interface for sending queries and commands, and publishing domain events.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a query to the appropriate handler and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the query handler.</typeparam>
    /// <param name="query">The query to send.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result.</returns>
    public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a command to the appropriate handler.
    /// </summary>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Send(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a command that returns a result to its corresponding handler.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the command handler.</typeparam>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the result.</returns>
    public Task<TResult> Send<TResult>(ICommand<TResult> command,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a domain event to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the domain event.</typeparam>
    /// <param name="notification">The domain event to publish.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task Publish<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}
