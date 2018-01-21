using System.Collections.Generic;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
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