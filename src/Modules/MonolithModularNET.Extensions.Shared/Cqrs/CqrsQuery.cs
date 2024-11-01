using MediatR;

namespace MonolithModularNET.Extensions.Shared.Cqrs;

public interface ICqrsQuery
{
    public DateTime? QueriedAt { get; set; }
}

public abstract class CqrsQuery<T>: IRequest<T>, ICqrsQuery
{
    public DateTime? QueriedAt { get; set; } = DateTime.UtcNow;
}