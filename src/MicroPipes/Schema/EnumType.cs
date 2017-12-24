using LanguageExt;

namespace MicroPipes.Schema
{
    public sealed class EnumType
    {
        public EnumType(bool isFlag, OrdinalType @base, Arr<EnumField> fields)
        {
            IsFlag = isFlag;
            Base = @base;
            Fields = fields;
        }

        public bool IsFlag { get; }
        public OrdinalType Base { get; }
        public Arr<EnumField> Fields { get; }

        private bool Equals(EnumType other)
        {
            return IsFlag == other.IsFlag && Base == other.Base && Fields.Equals(other.Fields);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is EnumType && Equals((EnumType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsFlag.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Base;
                hashCode = (hashCode * 397) ^ Fields.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(EnumType left, EnumType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EnumType left, EnumType right)
        {
            return !Equals(left, right);
        }
    }
}