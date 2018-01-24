using System;
using System.Collections.Generic;
using MicroPipes.Reflection;

namespace MicroPipes.Transport
{
    public sealed class EventConsumeEnd : PipeEnd
    {
        public EventConsumeEnd(IReadOnlyDictionary<QualifiedIdentifier, object> metadata, Type eventType) :
            base(metadata)
        {
            EventType = eventType;
        }

        public Type EventType { get; }
    }
}