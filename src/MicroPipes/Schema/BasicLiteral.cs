using System;

namespace MicroPipes.Schema
{
    public abstract class BasicLiteral : IEquatable<BasicLiteral>
    {
        public sealed class OrdLiteral : BasicLiteral
        {
            public OrdLiteral(OrdinalLiteral value)
            {
                Value = value;
            }

            public OrdinalLiteral Value { get; }
        }
        
        public sealed class F32Literal : BasicLiteral
        {
            public F32Literal(float value)
            {
                Value = value;
            }

            public float Value { get; }
        }
        
        public sealed class F64Literal : BasicLiteral
        {
            public F64Literal(double value)
            {
                Value = value;
            }

            public double Value { get; }
        }
        
        public sealed class StringLiteral : BasicLiteral
        {
            public StringLiteral(string value)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }

            public string Value { get; }
        }
        
        public sealed class UuidLiteral : BasicLiteral
        {
            public UuidLiteral(Guid value)
            {
                Value = value;
            }

            public Guid Value { get; }
        }
        
        public sealed class BoolLiteral : BasicLiteral
        {
            public BoolLiteral(bool value)
            {
                Value = value;
            }

            public bool Value { get; }
        }
        
        public sealed class DTLiteral : BasicLiteral
        {
            public DTLiteral(DateTime value)
            {
                Value = value;
            }

            public DateTime Value { get; }
        }
        
        public sealed class DTOLiteral : BasicLiteral
        {
            public DTOLiteral(DateTimeOffset value)
            {
                Value = value;
            }

            public DateTimeOffset Value { get; }
        }
        
        public sealed class TSLiteral : BasicLiteral
        {
            public TSLiteral(TimeSpan value)
            {
                Value = value;
            }

            public TimeSpan Value { get; }
        }
        
        public sealed class NoneLiteral : BasicLiteral
        {
            private NoneLiteral()
            {
            }
            
            public static readonly NoneLiteral Value = new NoneLiteral();
        }
    
        public static BasicLiteral Ordinal(OrdinalLiteral ordinal) => new OrdLiteral(ordinal);
        public static BasicLiteral F32(float value) => new F32Literal(value);
        public static BasicLiteral F64(double value) => new F64Literal(value);
        public static BasicLiteral String(string value) => new StringLiteral(value);
        public static BasicLiteral Uuid(Guid value) => new UuidLiteral(value);
        public static BasicLiteral Bool(bool value) => new BoolLiteral(value);
        public static BasicLiteral DT(DateTime value) => new DTLiteral(value);
        public static BasicLiteral DTO(DateTimeOffset value) => new DTOLiteral(value);
        public static BasicLiteral TS(TimeSpan value) => new TSLiteral(value);
        public static BasicLiteral None => NoneLiteral.Value;

        public bool Equals(BasicLiteral other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (other.GetType() != GetType()) return false;
            switch (this)
            {
                case OrdLiteral ord:
                    return ord.Value == ((OrdLiteral) other).Value;
                case F32Literal f32:
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    return f32.Value == ((F32Literal) other).Value;
                case F64Literal f64:
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    return f64.Value == ((F64Literal) other).Value;
                case StringLiteral str:
                    return str.Value == ((StringLiteral) other).Value;
                case UuidLiteral uuid:
                    return uuid.Value == ((UuidLiteral) other).Value;
                case BoolLiteral fl:
                    return fl.Value == ((BoolLiteral) other).Value;
                case DTLiteral dt:
                    return dt.Value == ((DTLiteral) other).Value;
                case DTOLiteral dto:
                    return dto.Value == ((DTOLiteral) other).Value;
                case TSLiteral ts:
                    return ts.Value == ((TSLiteral) other).Value;
                case NoneLiteral _:
                    return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BasicLiteral) obj);
        }

        public override int GetHashCode()
        {
            switch (this)
            {
                case OrdLiteral ord:
                    return ord.Value.GetHashCode();
                case F32Literal f32:
                    return f32.Value.GetHashCode();
                case F64Literal f64:
                    return f64.Value.GetHashCode();
                case StringLiteral str:
                    return str.Value.GetHashCode();
                case UuidLiteral uuid:
                    return uuid.Value.GetHashCode();
                case BoolLiteral fl:
                    return fl.Value.GetHashCode();
                case DTLiteral dt:
                    return dt.Value.GetHashCode();
                case DTOLiteral dto:
                    return dto.Value.GetHashCode();
                case TSLiteral ts:
                    return ts.Value.GetHashCode();
                case NoneLiteral _:
                    return typeof(NoneLiteral).GetHashCode();
            }

            return 0;
        }

        public static bool operator ==(BasicLiteral left, BasicLiteral right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BasicLiteral left, BasicLiteral right)
        {
            return !Equals(left, right);
        }
    }
}