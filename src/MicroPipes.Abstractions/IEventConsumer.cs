using System.Threading;
using System.Threading.Tasks;
using MicroPipes.Schema;

namespace MicroPipes
{
    public interface IEventConsumer<T>
    {
        Task<Acknowledge> HandleAsync(Message<T> message, CancellationToken cancellation);
    }
}