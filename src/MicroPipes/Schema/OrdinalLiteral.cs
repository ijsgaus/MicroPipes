using System;

namespace MicroPipes.Schema
{
    public abstract class OrdinalLiteral : IEquatable<OrdinalLiteral>
    {
        private OrdinalLiteral()
        {
        }

        public sealed class U8Literal : OrdinalLiteral
        {
            public U8Literal(byte value) => Value = value;

            public byte Value { get; }
            
        }
        
        public sealed class I8Literal : OrdinalLiteral
        {
            public I8Literal(sbyte value) => Value = value;

            public sbyte Value { get; }    
        }
        
        public sealed class U16Literal : OrdinalLiteral
        {
            public U16Literal(ushort value) => Value = value;

            public ushort Value { get; }    
        }
        
        public sealed class I16Literal : OrdinalLiteral
        {
            public I16Literal(short value) => Value = value;

            public short Value { get; }    
        }
        
        public sealed class U32Literal : OrdinalLiteral
        {
            public U32Literal(uint value) => Value = value;

            public uint Value { get; }    
        }
        
        public sealed class I32Literal : OrdinalLiteral
        {
            public I32Literal(int value) => Value = value;

            public int Value { get; }    
        }
        
        public sealed class U64Literal : OrdinalLiteral
        {
            public U64Literal(ulong value) => Value = value;

            public ulong Value { get; }    
        }
        
        public sealed class I64Literal : OrdinalLiteral
        {
            public I64Literal(long value) => Value = value;

            public long Value { get; }    
        }

        public static OrdinalLiteral U8(byte value) => new U8Literal(value);
        public static OrdinalLiteral I8(sbyte value) => new I8Literal(value);
        public static OrdinalLiteral U16(ushort value) => new U16Literal(value);
        public static OrdinalLiteral I16(short value) => new I16Literal(value);
        public static OrdinalLiteral U32(uint value) => new U32Literal(value);
        public static OrdinalLiteral I32(int value) => new I32Literal(value);
        public static OrdinalLiteral U64(ulong value) => new U64Literal(value);
        public static OrdinalLiteral I64(long value) => new I64Literal(value);
        
        public bool Equals(OrdinalLiteral other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (other.GetType() != GetType()) return false;
            switch (this)
            {
                case I16Literal i16:
                    return i16.Value == ((I16Literal) other).Value;
                case I32Literal i32:
                    return i32.Value == ((I32Literal) other).Value;
                case I64Literal i64:
                    return i64.Value == ((I64Literal) other).Value;
                case I8Literal i8:
                    return i8.Value == ((I8Literal) other).Value;
                case U16Literal u16:
                    return u16.Value == ((U16Literal) other).Value;
                case U32Literal u32:
                    return u32.Value == ((U32Literal) other).Value;
                case U64Literal u64:
                    return u64.Value == ((U64Literal) other).Value;
                case U8Literal u8:
                    return u8.Value == ((U8Literal) other).Value;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrdinalLiteral) obj);
        }

        public override int GetHashCode()
        {
            switch (this)
            {
                case I16Literal i16:
                    return i16.Value.GetHashCode();
                case I32Literal i32:
                    return i32.Value.GetHashCode();
                case I64Literal i64:
                    return i64.Value.GetHashCode();
                case I8Literal i8:
                    return i8.Value.GetHashCode();
                case U16Literal u16:
                    return u16.Value.GetHashCode();
                case U32Literal u32:
                    return u32.Value.GetHashCode();
                case U64Literal u64:
                    return u64.Value.GetHashCode();
                case U8Literal u8:
                    return u8.Value.GetHashCode();
            }
            return 0;
        }

        public static bool operator ==(OrdinalLiteral left, OrdinalLiteral right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OrdinalLiteral left, OrdinalLiteral right)
        {
            return !Equals(left, right);
        }
    }
}