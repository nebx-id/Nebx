using MediatR;

namespace Nebx.BuildingBlocks.AspNetCore.CQRS;

public interface IQuery<out TResult> : IRequest<TResult>;