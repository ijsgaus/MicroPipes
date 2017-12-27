using System;
using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IResponseSender<TResponse>
    {
        Task ResponseAsync(Message<TResponse> response);
        Task ResponseErrorAsync(Message<Exception> exception);
    }
}