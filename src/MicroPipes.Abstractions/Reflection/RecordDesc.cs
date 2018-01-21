using System.Collections.Generic;
using System.Dynamic;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public sealed class RecordDesc : ComplexTypeDesc
    {
        public RecordDesc([NotNull] QualifiedIdentifier name, IEnumerable<(NameAndIndex, TypeMemberDesc)> members) : base(name, members)
        {
        }

        internal override void Anchor()
        {
            
        }
    }
}