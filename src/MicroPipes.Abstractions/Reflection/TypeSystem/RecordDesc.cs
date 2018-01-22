using System.Collections.Generic;
using System.Dynamic;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public sealed class RecordDesc : ComplexTypeDesc
    {
        [CanBeNull]
        public QualifiedIdentifier BaseRecord { get; }

        public RecordDesc([NotNull] QualifiedIdentifier name, [CanBeNull] QualifiedIdentifier baseRecord, IEnumerable<KeyValuePair<NameAndIndex, TypeMemberDesc>> members) : base(name, members)
        {
            BaseRecord = baseRecord;
        }

        protected override bool Equals(TypeDesc other)
        {
            return base.Equals(other) && ((RecordDesc) other).BaseRecord == BaseRecord;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (BaseRecord?.GetHashCode() ?? 0);    
            }
            
        }

        internal override void Anchor()
        {
            
        }
    }
}