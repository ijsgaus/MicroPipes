using System.Collections.Generic;

namespace MicroPipes.Reflection
{
    public interface IReflectionExtensible
    {
        IReadOnlyDictionary<QualifiedIdentifier, object> Extensions { get; }
    }
}