using MediatR;
using Nebx.BuildingBlocks.AspNetCore.Models.DomainDriven;

namespace Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces;

public interface IQuery<out TResult> : IRequest<TResult>;

public interface IQueryHandler<in TQuery, TResult>
    : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>;

public interface ICommand : IRequest;

public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand> where TCommand : ICommand;

public interface INotification : MediatR.INotification;

public interface INotificationHandler<in TNotification>
    : MediatR.INotificationHandler<TNotification> where TNotification : INotification;

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
    Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a command to the appropriate handler.
    /// </summary>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Send(ICommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a domain event to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the domain event.</typeparam>
    /// <param name="notification">The domain event to publish.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Publish<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}