using System;
using JetBrains.Annotations;

namespace MicroPipes
{
    public class SourceId : IEquatable<SourceId>
    {
        public SourceId([NotNull] string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        [NotNull] public string Value { get; }


        public bool Equals(SourceId other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || string.Equals(Value, other.Value, StringComparison.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SourceId) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCulture.GetHashCode(Value);
        }

        public static bool operator ==(SourceId left, SourceId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SourceId left, SourceId right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator SourceId(string value)
        {
            return new SourceId(value);
        }
    }
}