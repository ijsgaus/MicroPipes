using System;

namespace MicroPipes
{
    public interface IResponderProvider<in TResponse> 
        where TResponse : class
    {
        IResponderProvider<TResponse> WithContext(ResponseContext context);
        IResponder<TResponse> WithTimeout(TimeSpan timeout); 
        
    }
}