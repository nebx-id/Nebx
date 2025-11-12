using LiteBus.Events.Abstractions;

namespace Nebx.BuildingBlocks.AspNetCore.Mediator.Abstractions;

/// <summary>
/// Represents a domain event or notification that can be published to multiple event handlers.
/// </summary>
public interface INotification : IEvent;