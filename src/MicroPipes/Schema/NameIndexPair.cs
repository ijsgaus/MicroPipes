using System;

namespace MicroPipes.Schema
{
    public class NameIndexPair : IEquatable<NameIndexPair>
    {
        public NameIndexPair(Identifier name, int index)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Index = index;
        }

        public Identifier Name { get; }
        public int Index { get; }

        public bool Equals(NameIndexPair other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Name, other.Name) || Index == other.Index;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NameIndexPair) obj);
        }

        public override int GetHashCode() => Index.GetHashCode();

        public static bool operator ==(NameIndexPair left, NameIndexPair right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NameIndexPair left, NameIndexPair right)
        {
            return !Equals(left, right);
        }
    }
}