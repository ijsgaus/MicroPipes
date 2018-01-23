using System;
using System.Threading;
using System.Threading.Tasks;
using MicroPipes.Engine;

namespace MicroPipes.Transport
{
    public interface IPipes
    {
        Task<IDisposable> ConnectListeners(TransportId transportId, IListenerConnector connector, CancellationToken cancellation);
    }
}