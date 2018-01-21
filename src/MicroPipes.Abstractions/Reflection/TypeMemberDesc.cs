using System.Collections.Generic;

namespace MicroPipes.Reflection
{
    public sealed class TypeMemberDesc : IReflectionExtensible
    {
        public TypeMemberDesc(QualifiedIdentifier value, IEnumerable<(QualifiedIdentifier, object)> extensions)
        {
            Value = value;
            Extensions = extensions.ToImmutableDictionary();
        }

        public QualifiedIdentifier  Value { get; }
        public IReadOnlyDictionary<QualifiedIdentifier, object> Extensions { get; }

        private bool Equals(TypeMemberDesc other)
        {
            return Equals(Value, other.Value) && Extensions.DefaultComparerEqual(other.Extensions);
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
                return (Value.GetHashCode() * 397) ^ Extensions.DefaultComparerHashCode();
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