using System;
using System.Collections.Generic;
using MicroPipes.Reflection;
using MicroPipes.Serialization;
using MicroPipes.TypeEncoding;

namespace MicroPipes.Transport
{
    public sealed class EventPublishEnd : PipeEnd
    {


        public EventPublishEnd(IReadOnlyDictionary<QualifiedIdentifier, object> metadata, Type eventType,
            ISerializatorProvider serializatorProvider, ITypeNameEncoding typeEncoding) : base(metadata)
        {
            EventType = eventType;
            SerializatorProvider = serializatorProvider;
            TypeEncoding = typeEncoding;

        }

        public Type EventType { get; }
        public ISerializatorProvider SerializatorProvider { get; }
        public ITypeNameEncoding TypeEncoding { get; }



    }
}