using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LiteBus.Queries.Abstractions;
using Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;
using Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;
using ICommand = Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator.ICommand;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;

/// <inheritdoc />
public class MediatorImplementation : IMediator
{
    private readonly ICommandMediator _commandMediator;
    private readonly IQueryMediator _queryMediator;
    private readonly IEventMediator _eventMediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatorImplementation"/> class.
    /// </summary>
    /// <param name="commandMediator">The LiteBus mediator responsible for handling commands.</param>
    /// <param name="queryMediator">The LiteBus mediator responsible for handling queries.</param>
    /// <param name="eventMediator">The LiteBus mediator responsible for publishing domain events.</param>
    public MediatorImplementation(
        ICommandMediator commandMediator,
        IQueryMediator queryMediator,
        IEventMediator eventMediator)
    {
        _commandMediator = commandMediator;
        _queryMediator = queryMediator;
        _eventMediator = eventMediator;
    }

    /// <inheritdoc />
    public async Task<TResult> Send<TResult>(Interfaces.Mediator.IQuery<TResult> query,
        CancellationToken cancellationToken = default)
        => await _queryMediator.QueryAsync(query, cancellationToken);

    /// <inheritdoc />
    public async Task Send(ICommand command, CancellationToken cancellationToken = default)
        => await _commandMediator.SendAsync(command, cancellationToken);

    /// <inheritdoc />
    public async Task<TResult> Send<TResult>(Interfaces.Mediator.ICommand<TResult> command,
        CancellationToken cancellationToken = default)
        => await _commandMediator.SendAsync(command, cancellationToken);

    /// <inheritdoc />
    public async Task Publish<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
        => await _eventMediator.PublishAsync(notification, cancellationToken);
}