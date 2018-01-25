using System;
using System.Threading.Tasks;

namespace MicroActors
{
    public delegate Func<Task<ActorFunc<TKey>>> ActorFunc<TKey>(object message, ActorContext<TKey> context);

    public delegate Func<Task<ActorFunc<TKey, TMessage>>> ActorFunc<TKey, TMessage>(Message<TMessage> message, ActorContext<TKey> context);

    public delegate Func<ActorFunc<TKey>> ActorProps<TKey>(IPropsContext<TKey> ctx);

    public delegate Func<ActorFunc<TKey, TMessage>> ActorProps<TKey, TMessage>(IPropsContext<TKey> ctx);

    public interface IPropsContext<TKey>
    {
    }

    public interface IActorKey<TKey>
    {
        
    }
}