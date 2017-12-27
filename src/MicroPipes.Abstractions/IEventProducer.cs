using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IEventProducer<T>
    {
        Task PublishAsync(Message<T> message);
    }
}