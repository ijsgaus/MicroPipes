using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroPipes.Transport
{
    public interface IEventProducer : IAsyncDisposable
    {
        Task PublishAsync(object message, SendContext context, CancellationToken cancellation);
    }
    
    
}