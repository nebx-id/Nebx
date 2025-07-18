using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Nebx.BuildingBlocks.AspNetCore.CQRS;
using Nebx.BuildingBlocks.AspNetCore.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations.EntityFramework;

/// <summary>
/// Intercepts EF Core's save changes operations to dispatch domain events after an aggregate root is modified.
/// </summary>
internal sealed class DispatchDomainEventInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;
    
    public DispatchDomainEventInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Intercepts the synchronous save changes operation to dispatch domain events after entities are persisted.
    /// </summary>
    /// <param name="eventData">The event data containing details about the save operation.</param>
    /// <param name="result">The result of the save operation.</param>
    /// <returns>The interception result of the save operation.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // eventual consistency
        var saveResult = base.SavingChanges(eventData, result);
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return saveResult;
    }

    /// <summary>
    /// Intercepts the asynchronous save changes operation to dispatch domain events after entities are persisted.
    /// </summary>
    /// <param name="eventData">The event data containing details about the save operation.</param>
    /// <param name="result">The result of the save operation.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation and the interception result of the save operation.</returns>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        // eventual consistency
        var saveResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        await DispatchDomainEvents(eventData.Context);
        return saveResult;
    }

    /// <summary>
    /// Extracts and dispatches domain events from aggregate roots tracked by the DbContext.
    /// </summary>
    /// <param name="context">The EF Core <see cref="DbContext"/> instance.</param>
    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context is null) return;

        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(a => a.DequeueEvents())
            .ToList();

        foreach (var domainEvent in domainEvents) await _mediator.Publish(domainEvent);
    }
}