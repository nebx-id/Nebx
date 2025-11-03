namespace Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;

/// <summary>
/// Handles a specific command.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public interface ICommandHandler<in TCommand>
    : LiteBus.Commands.Abstractions.ICommandHandler<TCommand> where TCommand : ICommand;