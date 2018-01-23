using System;
using System.Threading.Tasks;
using MicroPipes.Transport;

namespace MicroPipes.Engine
{
    public interface IListenerConnector
    {
        Task<IDisposable> Listen(IFatRequestListener listener);
    }
}