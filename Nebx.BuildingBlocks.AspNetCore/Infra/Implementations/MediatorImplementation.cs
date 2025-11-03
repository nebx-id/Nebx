using Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;
using Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;

/// <inheritdoc />
public class MediatorImplementation : IMediator
{
    private readonly MediatR.IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatorImplementation"/> class.
    /// </summary>
    /// <param name="mediator">The underlying mediator used to send requests and publish notifications.</param>
    public MediatorImplementation(MediatR.IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => await _mediator.Send(query, cancellationToken);

    /// <inheritdoc />
    public async Task Send(ICommand command, CancellationToken cancellationToken = default)
        => await _mediator.Send(command, cancellationToken);

    /// <inheritdoc />
    public async Task Publish<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
        => await _mediator.Publish(notification, cancellationToken);
}