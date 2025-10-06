using Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces;
using Nebx.BuildingBlocks.AspNetCore.Models.DomainDriven;

namespace Nebx.BuildingBlocks.AspNetCore.Infrastructure.Implementations;

public class MediatorImpl : IMediator
{
    private readonly MediatR.IMediator _mediator;

    public MediatorImpl(MediatR.IMediator mediator)
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