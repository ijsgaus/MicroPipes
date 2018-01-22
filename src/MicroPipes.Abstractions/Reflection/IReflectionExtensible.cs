using System.Collections.Generic;
using System.Collections.Immutable;

namespace MicroPipes.Reflection
{
    public interface IReflectionExtensible
    {
        IImmutableDictionary<QualifiedIdentifier, object> Extensions { get; }
    }
}