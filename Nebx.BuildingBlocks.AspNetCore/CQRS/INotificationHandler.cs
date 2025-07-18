namespace Nebx.BuildingBlocks.AspNetCore.CQRS;

public interface INotificationHandler<in TNotification> : MediatR.INotificationHandler<TNotification>
    where TNotification : INotification;