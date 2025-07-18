using Nebx.BuildingBlocks.AspNetCore.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.CQRS;

public interface IMediator
{
    Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    Task Send(ICommand command, CancellationToken cancellationToken = default);

    Task Publish<TEvent>(TEvent notification, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}