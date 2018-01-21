using System.Collections.Generic;
using System.Collections.Immutable;

namespace MicroPipes.Reflection
{
    public class EventEndpointDesc : EndpointDesc
    {

        public EventEndpointDesc(Identifier name, IEnumerable<QualifiedIdentifier> overloads = null,
            IEnumerable<(QualifiedIdentifier, object)> extensions = null) : base(name, extensions)
        {
            Overloads = overloads?.ToImmutableHashSet() ?? ImmutableHashSet<QualifiedIdentifier>.Empty;
        }

        public IReadOnlyCollection<QualifiedIdentifier> Overloads { get; }

        internal override bool Equals(EndpointDesc other)
        {
            return base.Equals(other) && Overloads.DefaultComparerEqual(((EventEndpointDesc) other).Overloads);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Overloads.DefaultComparerHashCode();
            }
        }
        
        
    }
}