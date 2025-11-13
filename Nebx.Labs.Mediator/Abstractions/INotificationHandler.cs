using LiteBus.Events.Abstractions;

namespace Nebx.Labs.Mediator.Abstractions;

/// <summary>
/// Handles a specific notification or event.
/// </summary>
/// <typeparam name="TNotification">The notification type.</typeparam>
public interface INotificationHandler<in TNotification>
    : IEventHandler<TNotification> where TNotification : INotification;