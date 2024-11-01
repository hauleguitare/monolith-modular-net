using MediatR;

namespace MonolithModularNET.Extensions.Shared.Cqrs;

public interface IMediatorHandler
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default);
    
    Task SendAsync(IRequest request,
        CancellationToken cancellationToken = default);

    Task PublishAsync(INotification @event, CancellationToken cancellationToken = default);

    IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default) where TResponse :  class, new();

}

public class MediatorHandler(IMediator mediator) : IMediatorHandler
{
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return mediator.Send(request, cancellationToken);
    }

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        return mediator.Send(request, cancellationToken);
    }

    public Task PublishAsync(INotification @event, CancellationToken cancellationToken = default)
    {
        return mediator.Publish(@event, cancellationToken);
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default) where TResponse :  class, new()
    {
        return mediator.CreateStream(request, cancellationToken);
    }
}