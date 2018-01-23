using System.Threading;
using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IEventReciver<TMsg> : IEventReciver<IEvent<TMsg>, TMsg> 
        where TMsg : class
    {
        
    }

    public interface IEventReciver<TEvent, in TMsg> : IEventReciver
        where TMsg : class
        where TEvent : IEvent<TMsg>
    {
        Task Handle(TMsg message, CancellationToken cancellation);
    }

    public interface IEventReciver
    {
        
    }
}