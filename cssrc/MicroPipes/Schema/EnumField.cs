namespace MicroPipes.Schema
{
    public sealed class EnumField
    {
        public EnumField(Identifier name, long value, string summary)
        {
            Name = name;
            Value = value;
            Summary = summary;
        }

        public Identifier Name { get; }
        public long Value { get; }
        public string Summary { get; }

        private bool Equals(EnumField other)
        {
            return Equals(Name, other.Name) && Value == other.Value && string.Equals(Summary, other.Summary);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is EnumField && Equals((EnumField) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Value.GetHashCode();
                hashCode = (hashCode * 397) ^ (Summary != null ? Summary.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(EnumField left, EnumField right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EnumField left, EnumField right)
        {
            return !Equals(left, right);
        }
    }
}