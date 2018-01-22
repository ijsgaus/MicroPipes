using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public abstract class ComplexTypeDesc : TypeDesc, IReflectionExtensible
    {
        

        public ComplexTypeDesc([NotNull] QualifiedIdentifier name, IEnumerable<KeyValuePair<NameAndIndex, TypeMemberDesc>> members,
            IEnumerable<KeyValuePair<QualifiedIdentifier, object>> extensions = null) : base(name)
        {
            Members = members is IImmutableDictionary<NameAndIndex, TypeMemberDesc> mid 
                ? mid
                : (members?.ToImmutableDictionary() ?? ImmutableDictionary<NameAndIndex, TypeMemberDesc>.Empty);
            Extensions = extensions is IImmutableDictionary<QualifiedIdentifier, object> eid
                ? eid
                : (extensions?.ToImmutableDictionary() ?? ImmutableDictionary<QualifiedIdentifier, object>.Empty);
        }

        public IImmutableDictionary<NameAndIndex, TypeMemberDesc> Members { get; }

        public IImmutableDictionary<QualifiedIdentifier, object> Extensions { get; }
        

        protected override bool Equals(TypeDesc other)
        {
            var typedOther = (ComplexTypeDesc) other;
            return base.Equals(other) && typedOther.Members.StructureEqual(Members) && typedOther.Extensions.StructureEqual(Extensions);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode() * 397;
                hashCode = hashCode ^ Members.StructureHashCode();
                hashCode = (hashCode * 397) ^ Extensions.StructureHashCode();
                return hashCode;
            }
        }
    }
}