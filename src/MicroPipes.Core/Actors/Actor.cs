using System;
using System.Threading.Tasks;

namespace MicroPipes.Actors
{
    public delegate Task<Func<object>> Actor(object message);
    
    public delegate Task<Func<T>> Actor<T>(T message);
}