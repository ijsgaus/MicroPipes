using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MicroPipes
{
    public class ReadOnlyCollectionEqualityComparer<T> : IEqualityComparer<IReadOnlyCollection<T>>
    {
        public bool Equals(IReadOnlyCollection<T> x, IReadOnlyCollection<T> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null) return false;
            if (y == null) return false;
            return x.Count == y.Count && x.All(y.Contains);
        }

        public int GetHashCode(IReadOnlyCollection<T> obj)
        {
            IEqualityComparer<T> comparer;
            switch (obj)
            {
                case ImmutableHashSet<T> ihs:
                    comparer = ihs.KeyComparer;
                    break;
                default:
                    comparer = EqualityComparer<T>.Default;
                    break;
            }

            return obj?
                .OrderBy(p => p)
                .Aggregate(0, (a, p) =>
                {
                    unchecked
                    {
                        return (a * 497) ^ (ReferenceEquals(p, null) ? 0 : comparer.GetHashCode(p));
                    }
                }) ?? 0;
        }
        
        public static readonly ReadOnlyCollectionEqualityComparer<T> Default = new ReadOnlyCollectionEqualityComparer<T>();
    }
}