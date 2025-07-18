using MediatR;

namespace Nebx.BuildingBlocks.AspNetCore.CQRS;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>;