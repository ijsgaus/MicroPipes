using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public class EnumDesc : TypeDesc, IReflectionExtensible
    {
        public EnumDesc([NotNull] QualifiedIdentifier name, IEnumerable<(NameAndIndex, int)> fields,
            IEnumerable<(QualifiedIdentifier, object)> extensions) : base(name)
        {
            Fields = fields?.ToImmutableDictionary() ?? ImmutableDictionary<NameAndIndex, int>.Empty;
            Extensions = extensions.ToImmutableDictionary();
        }

        public IReadOnlyDictionary<NameAndIndex, int> Fields { get; }

        public IReadOnlyDictionary<QualifiedIdentifier, object> Extensions { get; }

        protected internal override bool Equals(TypeDesc other)
        {
            var enumDesc = (EnumDesc) other;
            return base.Equals(other) && Fields.DefaultComparerEqual(enumDesc.Fields) &&
                Extensions.DefaultComparerEqual(enumDesc.Extensions);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hc = base.GetHashCode();
                hc = (hc * 397) ^ Fields.DefaultComparerHashCode();
                hc = (hc * 397) ^ Extensions.DefaultComparerHashCode();
                return hc;
            }
        }

        internal override void Anchor()
        {
            
        }
    }
}