using System;
using System.Threading.Tasks;

namespace MicroPipes
{
    public interface IAsyncDisposable : IDisposable
    {
        bool IsDisposed { get; }
        Task DisposeAsync();
    }
}