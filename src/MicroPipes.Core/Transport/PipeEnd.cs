using System.Collections.Generic;
using MicroPipes.Reflection;

namespace MicroPipes.Transport
{
    public abstract class PipeEnd
    {
        protected PipeEnd(IReadOnlyDictionary<QualifiedIdentifier, object> metadata)
        {
            Metadata = metadata;
        }

        public IReadOnlyDictionary<QualifiedIdentifier, object> Metadata { get; }
    }
}