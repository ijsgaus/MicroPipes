using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using MicroPipes.Reflection;
using MicroPipes.Serialization;


namespace MicroPipes.Transport
{
    public interface IPipes
    {
        Task<IDisposable> ConnectPipes(TransportId transportId, IPointConnector connector, CancellationToken cancellation);
    }

    public interface IPointConnector
    {
        Task<IDisposable> ConnectPipe(PipeEnd pipeEnd);
    }

    public abstract class PipeEnd
    {
        protected PipeEnd(IReadOnlyDictionary<QualifiedIdentifier, object> metadata)
        {
            Metadata = metadata;
        }

        public IReadOnlyDictionary<QualifiedIdentifier, object> Metadata { get; }
        
        
        public sealed class EventPublisherType : PipeEnd
        {
            

            public EventPublisherType(IReadOnlyDictionary<QualifiedIdentifier, object> metadata, Type eventType,
                ISerializatorProvider serializatorProvider, ITypeEncoding typeEncoding) : base(metadata)
            {
                EventType = eventType;
                SerializatorProvider = serializatorProvider;
                TypeEncoding = typeEncoding;
            
            }

            public Type EventType { get; }
            public ISerializatorProvider SerializatorProvider { get; }
            public ITypeEncoding TypeEncoding { get; }
            
            
            
        }

        public sealed class EventConsumerType : PipeEnd
        {
            public EventConsumerType(IReadOnlyDictionary<QualifiedIdentifier, object> metadata, Type eventType) : base(metadata)
            {
                EventType = eventType;
            }

            public Type EventType { get; }
        }
    }

    public interface ITypeEncoding
    {
    }
}