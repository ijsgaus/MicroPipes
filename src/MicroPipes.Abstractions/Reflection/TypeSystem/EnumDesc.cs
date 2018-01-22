using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public sealed class EnumDesc : TypeDesc, IReflectionExtensible
    {
        public EnumDesc([NotNull] QualifiedIdentifier name, 
            IEnumerable<KeyValuePair<Identifier, EnumFieldValue>> fields,
            IEnumerable<KeyValuePair<QualifiedIdentifier, object>> extensions = null) : base(name)
        {
            Fields = 
                fields is IImmutableDictionary<Identifier, EnumFieldValue> fid 
                    ? fid
                    : (fields?.ToImmutableDictionary() ?? ImmutableDictionary<Identifier, EnumFieldValue>.Empty);
            Extensions = 
                extensions is IImmutableDictionary<QualifiedIdentifier, object> id 
                    ? id 
                    : (extensions?.ToImmutableDictionary() ?? ImmutableDictionary<QualifiedIdentifier, object>.Empty); 
        }

        public IImmutableDictionary<Identifier, EnumFieldValue> Fields { get; }

        public IImmutableDictionary<QualifiedIdentifier, object> Extensions { get; }

        protected override bool Equals(TypeDesc other)
        {
            var enumDesc = (EnumDesc) other;
            return base.Equals(other) && Fields.StructureEqual(enumDesc.Fields) &&
                Extensions.StructureEqual(enumDesc.Extensions);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hc = base.GetHashCode();
                hc = (hc * 397) ^ Fields.StructureHashCode();
                hc = (hc * 397) ^ Extensions.StructureHashCode();
                return hc;
            }
        }

        internal override void Anchor()
        {
            
        }
    }
}