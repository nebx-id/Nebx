using MediatR;

namespace Nebx.BuildingBlocks.AspNetCore.CQRS;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand;