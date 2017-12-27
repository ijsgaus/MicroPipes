using System;
using System.Text.RegularExpressions;

namespace MicroPipes.Schema
{
    public class Identifier : IEquatable<Identifier>, IComparable<Identifier>, IComparable
    {
        private static readonly Regex Regex = new Regex("^[A-Za-z][A-Za-z0-9_]*$");


        internal Identifier(string value)
        {
            Value = value;
        }

        private string Value { get; }

        public bool Equals(Identifier other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) ||
                   string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool TryParse(string value, out Identifier identifier)
        {
            identifier = null;
            if (value == null || !Regex.IsMatch(value))
                return false;
            identifier = new Identifier(value);
            return true;
        }

        public static Identifier Parse(string value)
        {
            return TryParse(value, out var id) ? id : throw new ArgumentException($"Invalid identifier '{value}'");
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(Value);
        }

        public static bool operator ==(Identifier left, Identifier right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Identifier left, Identifier right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }

        public int CompareTo(Identifier other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            return string.Compare(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public int CompareTo(object obj)
        {
            if (obj is null) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is Identifier)) throw new ArgumentException($"Object must be of type {nameof(Identifier)}");
            return CompareTo((Identifier) obj);
        }
    }
}