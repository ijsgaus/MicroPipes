namespace MicroPipes.Schema
{
    public abstract class BasicType
    {
        public sealed class OrdinalType : BasicType
        {
            public OrdinalType(Schema.OrdinalType value)
            {
                Value = value;
            }

            public Schema.OrdinalType Value { get; }
        }

        public sealed class F32Type : BasicType
        {
            internal F32Type() { }
        }
        
        public sealed class F64Type : BasicType
        {
            internal F64Type() { }
        }
        
        public sealed class StringType : BasicType
        {
            internal StringType() { }
        }
        
        public sealed class GuidType : BasicType
        {
            internal GuidType() { }
        }
        
        public sealed class DateTimeType : BasicType
        {
            internal DateTimeType() { }
        }
        
        public sealed class DateTimeOffsetType : BasicType
        {
            internal DateTimeOffsetType() { }
        }
        
        public sealed class TimeSpanType : BasicType
        {
            internal TimeSpanType() { }
        }
        
        public sealed class BoolType : BasicType
        {
            internal BoolType() { }
        }
        
        public sealed class UrlType : BasicType
        {
            internal UrlType() { }
        }
        
        public static BasicType Ordinal(Schema.OrdinalType ordinal) => new OrdinalType(ordinal);
        public static BasicType F32 = new F32Type();
        public static BasicType F64 = new F64Type();
        public static BasicType String = new StringType();
        public static BasicType Guid = new GuidType();
        public static BasicType DateTime = new DateTimeType();
        public static BasicType DateTimeOffset = new DateTimeOffsetType();
        public static BasicType TimeSpan = new TimeSpanType();
        public static BasicType Bool = new BoolType();
        public static BasicType Url = new UrlType();

        public static implicit operator BasicType(Schema.OrdinalType ord) => Ordinal(ord);
        
        

        protected bool Equals(BasicType other)
        {
            switch (this)
            {
                case OrdinalType ordinalType:
                    return ((OrdinalType) other).Value == ordinalType.Value; 
                default:
                    return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BasicType) obj);
        }

        public override int GetHashCode()
        {
            switch (this)
            {
                case OrdinalType ordinalType:
                    return ordinalType.Value.GetHashCode(); 
                default:
                    return GetType().GetHashCode();
            }
        }

        public static bool operator ==(BasicType left, BasicType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BasicType left, BasicType right)
        {
            return !Equals(left, right);
        }

        private BasicType()
        {
        }
    }
}