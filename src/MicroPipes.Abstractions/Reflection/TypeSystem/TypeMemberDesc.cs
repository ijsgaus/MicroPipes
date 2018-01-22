using System.Collections.Generic;
using System.Collections.Immutable;

namespace MicroPipes.Reflection
{
    public sealed class TypeMemberDesc : IReflectionExtensible
    {
        public TypeMemberDesc(QualifiedIdentifier value, IEnumerable<KeyValuePair<QualifiedIdentifier, object>> extensions = null)
        {
            Value = value;
            Extensions = extensions is IImmutableDictionary<QualifiedIdentifier, object> id
                ? id
                : (extensions?.ToImmutableDictionary() ?? ImmutableDictionary<QualifiedIdentifier, object>.Empty);
        }

        public QualifiedIdentifier  Value { get; }
        public IImmutableDictionary<QualifiedIdentifier, object> Extensions { get; }

        private bool Equals(TypeMemberDesc other)
        {
            return Equals(Value, other.Value) && Extensions.StructureEqual(other.Extensions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TypeMemberDesc) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ Extensions.StructureHashCode();
            }
        }

        public static bool operator ==(TypeMemberDesc left, TypeMemberDesc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TypeMemberDesc left, TypeMemberDesc right)
        {
            return !Equals(left, right);
        }
    }
}