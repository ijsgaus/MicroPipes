using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public interface IReflectionExtensible
    {
        [NotNull]
        IImmutableDictionary<QualifiedIdentifier, object> Extensions { get; }
    }
}