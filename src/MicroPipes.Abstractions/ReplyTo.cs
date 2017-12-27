using System;
using JetBrains.Annotations;

namespace MicroPipes
{
    public class ReplyTo : IEquatable<ReplyTo>
    {
        public ReplyTo([NotNull] string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        [NotNull] public string Value { get; }

        public bool Equals(ReplyTo other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || string.Equals(Value, other.Value, StringComparison.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ReplyTo) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCulture.GetHashCode(Value);
        }

        public static bool operator ==(ReplyTo left, ReplyTo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReplyTo left, ReplyTo right)
        {
            return !Equals(left, right);
        }
    }
}