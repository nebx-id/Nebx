using LiteBus.Events.Abstractions;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;

/// <summary>
/// Handles a specific notification or event.
/// </summary>
/// <typeparam name="TNotification">The notification type.</typeparam>
public interface INotificationHandler<in TNotification>
    : IEventHandler<TNotification> where TNotification : INotification;