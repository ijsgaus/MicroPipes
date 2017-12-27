using System;
using System.Data.Common;

namespace MicroPipes.Schema
{
    public abstract class BasicLiteral
    {
        public sealed class SignedOrdinalLiteral : BasicLiteral
        {
            public SignedOrdinalLiteral(long value) => Value = value;

            public long Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((SignedOrdinalLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class UnsignedOrdinalLiteral : BasicLiteral
        {
            public UnsignedOrdinalLiteral(ulong value) => Value = value;

            public ulong Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((UnsignedOrdinalLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class FloatLiteral : BasicLiteral
        {
            public FloatLiteral(double value) => Value = value;

            public double Value { get; }

            protected override bool Equals(BasicLiteral other)
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                => ((FloatLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class StringLiteral : BasicLiteral
        {
            public StringLiteral(string value) => Value = value;

            public string Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((StringLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class GuidLiteral : BasicLiteral
        {
            public GuidLiteral(Guid value) => Value = value;

            public Guid Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((GuidLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class BoolLiteral : BasicLiteral
        {
            public BoolLiteral(bool value) => Value = value;

            public bool Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((BoolLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class DateTimeLiteral : BasicLiteral
        {
            public DateTimeLiteral(DateTime value) => Value = value;

            public DateTime Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((DateTimeLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class DateTimeOffsetLiteral : BasicLiteral
        {
            public DateTimeOffsetLiteral(DateTimeOffset value) => Value = value;

            public DateTimeOffset Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((DateTimeOffsetLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class TimeSpanLiteral : BasicLiteral
        {
            public TimeSpanLiteral(TimeSpan value) => Value = value;

            public TimeSpan Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((TimeSpanLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class IdentifierLiteral : BasicLiteral
        {
            public IdentifierLiteral(QualifiedIdentifier value) => Value = value;

            public QualifiedIdentifier Value { get; }

            protected override bool Equals(BasicLiteral other)
                => ((IdentifierLiteral) other).Value == Value;

            public override int GetHashCode() => Value.GetHashCode();
        }
        
        public sealed class NoneLiteral : BasicLiteral
        {
            private NoneLiteral()
            {
            }

            protected override bool Equals(BasicLiteral other)
                => true;

            public override int GetHashCode() => typeof(NoneLiteral).GetHashCode();
            
            public static NoneLiteral NoneValue = new NoneLiteral(); 
        }
        
        public static BasicLiteral Signed(long value) => new SignedOrdinalLiteral(value);
        public static BasicLiteral Unsigned(ulong value) => new UnsignedOrdinalLiteral(value);
        public static BasicLiteral Float(double value) => new FloatLiteral(value);
        public static BasicLiteral String(string value) => new StringLiteral(value);
        public static BasicLiteral Guid(Guid value) => new GuidLiteral(value);
        public static BasicLiteral Bool(bool value) => new BoolLiteral(value);
        public static BasicLiteral DateTime(DateTime value) => new DateTimeLiteral(value);
        public static BasicLiteral DateTimeOffset(DateTimeOffset value) => new DateTimeOffsetLiteral(value);
        public static BasicLiteral TimeSpan(TimeSpan value) => new TimeSpanLiteral(value);
        public static BasicLiteral Id(QualifiedIdentifier value) => new IdentifierLiteral(value);
        public static BasicLiteral None => NoneLiteral.NoneValue;

        public static implicit operator BasicLiteral(byte value) => Unsigned(value);
        public static implicit operator BasicLiteral(ushort value) => Unsigned(value);
        public static implicit operator BasicLiteral(uint value) => Unsigned(value);
        public static implicit operator BasicLiteral(ulong value) => Unsigned(value);
        public static implicit operator BasicLiteral(sbyte value) => Signed(value);
        public static implicit operator BasicLiteral(short value) => Signed(value);
        public static implicit operator BasicLiteral(int value) => Signed(value);
        public static implicit operator BasicLiteral(long value) => Signed(value);
        
        public static implicit operator BasicLiteral(float value) => Float(value);
        public static implicit operator BasicLiteral(double value) => Float(value);
        
        public static implicit operator BasicLiteral(string value) => String(value);
        public static implicit operator BasicLiteral(Guid value) => Guid(value);
        public static implicit operator BasicLiteral(bool value) => Bool(value);
        public static implicit operator BasicLiteral(DateTime value) => DateTime(value);
        public static implicit operator BasicLiteral(DateTimeOffset value) => DateTimeOffset(value);
        public static implicit operator BasicLiteral(TimeSpan value) => TimeSpan(value);
        public static implicit operator BasicLiteral(Identifier value) => Id(new QualifiedIdentifier(value));
        public static implicit operator BasicLiteral(QualifiedIdentifier value) => Id(value);


        protected abstract bool Equals(BasicLiteral other);
        

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BasicLiteral) obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(BasicLiteral left, BasicLiteral right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BasicLiteral left, BasicLiteral right)
        {
            return !Equals(left, right);
        }


        private BasicLiteral()
        {
        }
    }
}