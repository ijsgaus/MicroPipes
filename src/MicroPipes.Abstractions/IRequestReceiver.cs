using System.Threading;
using System.Threading.Tasks;
using MicroPipes.Schema;

namespace MicroPipes
{
    public interface IRequestReceiver<TRequest, in TSender, TResponse>
        where TSender : IResponseSender<TResponse>
    {
        Task<Acknowledge> HandleAsync(Message<TRequest> request, TSender responder, CancellationToken cancellation);
    }

    public interface IRequestReceiver<TRequest, TResponse> 
        : IRequestReceiver<TRequest, IResponseSender<TResponse>, TResponse>
    {

    }
}