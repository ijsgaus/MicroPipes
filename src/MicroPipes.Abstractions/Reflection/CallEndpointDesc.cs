using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public class CallEndpointDesc : EndpointDesc
    {
        public CallEndpointDesc([NotNull] Identifier name) : this(name, new (QualifiedIdentifier, QualifiedIdentifier)[0])
        {
        }

        public CallEndpointDesc([NotNull] Identifier name,
            IEnumerable<KeyValuePair<QualifiedIdentifier, QualifiedIdentifier>> overloads,
            IEnumerable<(QualifiedIdentifier, object)> extensions = null)
            : base(name, extensions)
        {
            Overloads =
                    overloads == null 
                    ? ImmutableDictionary<QualifiedIdentifier, QualifiedIdentifier>.Empty 
                    : 
                        overloads is ImmutableDictionary<QualifiedIdentifier, QualifiedIdentifier> id
                        ? id
                        : ImmutableDictionary.CreateRange(overloads);
        }

        public CallEndpointDesc([NotNull] Identifier name,IEnumerable<(QualifiedIdentifier, QualifiedIdentifier)> overloads,
            IEnumerable<(QualifiedIdentifier, object)> extensions = null)
            : base(name, extensions)
        {
            Overloads = overloads.ToImmutableDictionary();
        }


        public IReadOnlyDictionary<QualifiedIdentifier, QualifiedIdentifier> Overloads { get; }

        internal override bool Equals(EndpointDesc other)
        {
            return base.Equals(other) && ((CallEndpointDesc) other).Overloads.DefaultComparerEqual(Overloads);
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() * 397) ^ Overloads.DefaultComparerHashCode();
        }
    }
}