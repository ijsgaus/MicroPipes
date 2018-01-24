using System;
using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IAsyncDisposable : IDisposable
    {
        Task<bool> IsDisposed { get; }

        Task DisposeAsync();
    }
}