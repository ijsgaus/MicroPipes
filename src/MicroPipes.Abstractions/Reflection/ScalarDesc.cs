using System;

namespace MicroPipes.Reflection
{
    public sealed class ScalarDesc : TypeDesc
    {
        public static QualifiedIdentifier ToIdentifier(ScalarKind kind)
        {
            switch (kind)
            {
                case ScalarKind.Unit:
                    return "unit";
                case ScalarKind.Bool:
                    return "bool";
                case ScalarKind.U8:
                    return "u8";
                case ScalarKind.I8:
                    return "i8";
                case ScalarKind.U16:
                    return "u16";
                case ScalarKind.I16:
                    return "i16";
                case ScalarKind.U32:
                    return "u32";
                case ScalarKind.I32:
                    return "i32";
                case ScalarKind.U64:
                    return "u64";
                case ScalarKind.I64:
                    return "i64";
                case ScalarKind.F32:
                    return "f32";
                case ScalarKind.F64:
                    return "f64";
                case ScalarKind.String:
                    return "string";
                case ScalarKind.DateTime:
                    return "datetime";
                case ScalarKind.TimeSpan:
                    return "timespan";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
        
        public ScalarDesc(ScalarKind kind) : base(ToIdentifier(kind))
        {
            Kind = kind;
        }

        public ScalarKind Kind { get; }

        protected internal override bool Equals(TypeDesc other)
        {
            return ((ScalarDesc) other).Kind == Kind;
        }

        public override int GetHashCode() => Kind.GetHashCode();

        internal override void Anchor() 
        {
            
        }
    }
}