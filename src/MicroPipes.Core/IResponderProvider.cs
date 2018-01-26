using System;

namespace MicroPipes
{
    public interface IResponderProvider<in TResponse> 
        where TResponse : class
    {
        IResponderProvider<TResponse> WithContext(ReceiveContext context);
        IResponder<TResponse> WithTimeout(TimeSpan timeout); 
        
    }
}