using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;


namespace MicroPipes.Transport
{
    public interface IPipes
    {
        Task<IDisposable> ConnectPipes(TransportId transportId, IPointConnector connector,
            CancellationToken cancellation);
    }
}