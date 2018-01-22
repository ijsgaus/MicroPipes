using System.Collections.Generic;
using System.Collections.Immutable;

namespace MicroPipes.Reflection
{
    public sealed class EnumFieldValue : IReflectionExtensible
    {
        public EnumFieldValue(int value, IEnumerable<KeyValuePair<QualifiedIdentifier, object>> extensions = null)
        {
            Value = value;
            Extensions = extensions is IImmutableDictionary<QualifiedIdentifier, object> id 
                ? id 
                : (extensions?.ToImmutableDictionary() ?? ImmutableDictionary<QualifiedIdentifier, object>.Empty);
        }

        public int Value { get; }

        public IImmutableDictionary<QualifiedIdentifier, object> Extensions { get; }

        private bool Equals(EnumFieldValue other)
        {
            return Value == other.Value && Extensions.StructureEqual(other.Extensions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is EnumFieldValue value && Equals(value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value * 397) ^ Extensions.StructureHashCode();
            }
        }

        public static bool operator ==(EnumFieldValue left, EnumFieldValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EnumFieldValue left, EnumFieldValue right)
        {
            return !Equals(left, right);
        }
    }
}