using System.Collections.Generic;
using System.Dynamic;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public abstract class ComplexTypeDesc : TypeDesc
    {
        public ComplexTypeDesc([NotNull] QualifiedIdentifier name, IEnumerable<(NameAndIndex, TypeMemberDesc)> members) : base(name)
        {
            Members = members.ToImmutableDictionary();
        }

        public IReadOnlyDictionary<NameAndIndex, TypeMemberDesc> Members { get; }

        protected internal override bool Equals(TypeDesc other)
        {
            return base.Equals(other) && ((ComplexTypeDesc) other).Members.DefaultComparerEqual(Members);
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() * 397) ^ Members.DefaultComparerHashCode();
        }
    }
    
    public sealed class RecordDesc : ComplexTypeDesc
    {
        public RecordDesc([NotNull] QualifiedIdentifier name, IEnumerable<(NameAndIndex, TypeMemberDesc)> members) : base(name, members)
        {
        }

        internal override void Anchor()
        {
            
        }
    }

    public sealed class OneOfType : ComplexTypeDesc
    {
        public OneOfType([NotNull] QualifiedIdentifier name, IEnumerable<(NameAndIndex, TypeMemberDesc)> members) : base(name, members)
        {
        }

        internal override void Anchor()
        {
            
        }
    }
}