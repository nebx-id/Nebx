using Nebx.BuildingBlocks.AspNetCore.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.CQRS;

public class MediatorImpl : IMediator
{
    private readonly MediatR.IMediator _mediator;

    public MediatorImpl(MediatR.IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => await _mediator.Send(query, cancellationToken);

    public async Task Send(ICommand command, CancellationToken cancellationToken = default)
        => await _mediator.Send(command, cancellationToken);

    public async Task Publish<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
        => await _mediator.Publish(notification, cancellationToken);
}