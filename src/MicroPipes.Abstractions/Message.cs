using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace MicroPipes
{
    
    public class Message<T>
    {
        public Message([CanBeNull] T payload, [CanBeNull] SourceId sender, [CanBeNull] CorrelationId correlationId, [CanBeNull]ReplyTo replyTo)
        {
            Payload = payload;
            Sender = sender;
            CorrelationId = correlationId;
            ReplyTo = replyTo;
        }

        [CanBeNull]
        public T Payload { get; }

        [CanBeNull]
        public SourceId Sender { get; }

        [CanBeNull]
        public CorrelationId CorrelationId { get; }

        [CanBeNull]
        public ReplyTo ReplyTo { get; }

        
        [SuppressMessage("ReSharper", "ParameterHidesMember")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Message<T> With(OptionalParam<SourceId> Sender = default,
            OptionalParam<CorrelationId> CorrelationId = default, OptionalParam<ReplyTo> ReplyTo = default)
            => new Message<T>(Payload, Sender.NoValue(this.Sender), CorrelationId.NoValue(this.CorrelationId), ReplyTo.NoValue(this.ReplyTo));

        public Message<TOther> Map<TOther>(Func<T, TOther> mapper) 
            => new Message<TOther>(mapper(Payload), Sender, CorrelationId, ReplyTo);
            
    }
}