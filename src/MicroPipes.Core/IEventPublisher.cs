using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IEventPublisher<TMsg> : IEventPublisher<IEvent<TMsg>, TMsg>
        where TMsg : class
    {
        
    }

    public interface IEventPublisher<TEvent, in TMsg> : IEventPublisher
        where TMsg : class
        where TEvent : IEvent<TMsg>
    {
        Task PublishAsync(TMsg message, SendContext sendContext, CancellationToken cancellation = default);
        IEventPublisher<TEvent, TMsg> WithTtl(TimeSpan timeSpan);
    }

    public interface IEventPublisher
    {
        
    }
}