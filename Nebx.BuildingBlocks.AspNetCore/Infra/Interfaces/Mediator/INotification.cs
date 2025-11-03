namespace Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;

/// <summary>
/// Represents a notification or event that can be published to multiple handlers.
/// </summary>
public interface INotification : MediatR.INotification;