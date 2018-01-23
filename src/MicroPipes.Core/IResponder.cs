using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IResponder<in TResponse> 
        where TResponse : class
    {
        IResponder<TResponse> WithContext(ResponseContext context);
        IResponder<TResponse> WithTimeout(TimeSpan timeout); 
        Task Send(TResponse response, CancellationToken token);
    }
}