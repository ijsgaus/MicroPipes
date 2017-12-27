using System.Threading;
using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IRequestHandler<TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(Message<TRequest> request, CancellationToken token);
    }
}