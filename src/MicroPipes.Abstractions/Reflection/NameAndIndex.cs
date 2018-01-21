using System;
using JetBrains.Annotations;

namespace MicroPipes.Reflection
{
    public class NameAndIndex
    {
        public NameAndIndex([NotNull] Identifier name, int index)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Index = index;
        }

        public Identifier Name { get; }
        public int Index { get; }

        protected bool Equals(NameAndIndex other)
        {
            return Equals(Name, other.Name) || Index == other.Index;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NameAndIndex) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Index;
            }
        }

        public static bool operator ==(NameAndIndex left, NameAndIndex right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NameAndIndex left, NameAndIndex right)
        {
            return !Equals(left, right);
        }
    }
}