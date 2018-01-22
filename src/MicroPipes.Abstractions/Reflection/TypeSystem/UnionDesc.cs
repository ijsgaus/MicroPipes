using System.Collections.Generic;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public sealed class UnionDesc : ComplexTypeDesc
    {
        public UnionDesc([NotNull] QualifiedIdentifier name, IEnumerable<KeyValuePair<NameAndIndex, TypeMemberDesc>> members,
            IEnumerable<KeyValuePair<QualifiedIdentifier, object>> extensions = null) : base(name, members, extensions)
        {
        }

        internal override void Anchor()
        {
            
        }
    }
}