using MediatR;

namespace MonolithModularNET.Extensions.Shared.Cqrs;

public abstract class CqrsQueryHandler<TQuery, TResponse>: IRequestHandler<TQuery, TResponse> where TQuery : CqrsQuery<TResponse>
{
    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
}