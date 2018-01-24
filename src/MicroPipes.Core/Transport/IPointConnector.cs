using System.Threading.Tasks;

namespace MicroPipes.Transport
{
    public interface IPointConnector
    {
        Task<IEventProducer> ConnectPipe(EventPublishEnd pipeEnd);
        Task<IAsyncDisposable> ConnectPipe(EventConsumeEnd pipeEnd);
    }
}