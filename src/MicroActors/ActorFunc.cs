using System;

namespace MicroActors
{
    public delegate Func<object, ActorContext> ActorFunc(object message, ActorContext context);

    public delegate Func<TMessage, ActorContext<TKey>> ActorFunc<TKey, TMessage>(Message<TMessage> message, ActorContext<TKey> context);
}