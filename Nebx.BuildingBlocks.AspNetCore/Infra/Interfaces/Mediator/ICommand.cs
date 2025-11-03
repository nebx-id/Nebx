using MediatR;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;

/// <summary>
/// Represents a command that performs an action without returning a result.
/// </summary>
public interface ICommand : IRequest;