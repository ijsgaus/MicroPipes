using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public abstract class EndpointDesc : IReflectionExtensible
    {
        protected EndpointDesc([NotNull] Identifier name, [CanBeNull] IEnumerable<(QualifiedIdentifier, object)> extensions)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Extensions = extensions.ToImmutableDictionary();
        }

        public Identifier Name { get; }

        public IReadOnlyDictionary<QualifiedIdentifier, object> Extensions { get; }

        internal virtual bool Equals(EndpointDesc other)
        {
            return Equals(Name, other.Name) && Extensions.DefaultComparerEqual(other.Extensions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EndpointDesc) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Extensions.DefaultComparerHashCode();
            }
        }

        public static bool operator ==(EndpointDesc left, EndpointDesc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EndpointDesc left, EndpointDesc right)
        {
            return !Equals(left, right);
        }
    }
}