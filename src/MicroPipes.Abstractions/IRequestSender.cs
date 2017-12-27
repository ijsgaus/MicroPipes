using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IRequestSender<TRequest>
    {
        Task RequestAsync(Message<TRequest> request);
    }
}