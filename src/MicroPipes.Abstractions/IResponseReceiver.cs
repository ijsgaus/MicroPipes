using System;
using System.Threading;
using System.Threading.Tasks;
using MicroPipes.Schema;

namespace MicroPipes
{
    public interface IResponseReceiver<TResponse>
    {
        Task<Acknowledge> HandleAsync(Func<Message<TResponse>> messageProvider, CancellationToken cancellation);
    }
}