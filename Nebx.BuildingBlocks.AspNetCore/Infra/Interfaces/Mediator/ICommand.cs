namespace Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;

/// <summary>
/// Represents a command that performs an action without returning a result.
/// </summary>
public interface ICommand : LiteBus.Commands.Abstractions.ICommand;

/// <summary>
/// Represents a command that performs an action and returns a result of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of result returned by the command handler.</typeparam>
public interface ICommand<TResult> : LiteBus.Commands.Abstractions.ICommand<TResult>;