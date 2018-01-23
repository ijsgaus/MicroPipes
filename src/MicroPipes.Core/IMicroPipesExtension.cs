using System;

namespace MicroPipes
{
    public interface IMicroPipesExtension : IServiceProvider
    {
        string Name { get; }
    }
}