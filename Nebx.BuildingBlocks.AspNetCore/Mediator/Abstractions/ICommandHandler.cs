namespace Nebx.BuildingBlocks.AspNetCore.Mediator.Abstractions;

/// <summary>
/// Handles a specific command.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public interface ICommandHandler<in TCommand>
    : LiteBus.Commands.Abstractions.ICommandHandler<TCommand> where TCommand : ICommand;

/// <summary>
/// Handles a command that performs an action and returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TResult">The result type returned by the handler.</typeparam>
public interface ICommandHandler<in TCommand, TResult>
    : LiteBus.Commands.Abstractions.ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>;