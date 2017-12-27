using System;
using JetBrains.Annotations;

namespace MicroPipes
{
    public class CorrelationId : IEquatable<CorrelationId>
    {
        public CorrelationId([NotNull] string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        [NotNull] public string Value { get; }

        public bool Equals(CorrelationId other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || string.Equals(Value, other.Value, StringComparison.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CorrelationId) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCulture.GetHashCode(Value);
        }

        public static bool operator ==(CorrelationId left, CorrelationId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CorrelationId left, CorrelationId right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}