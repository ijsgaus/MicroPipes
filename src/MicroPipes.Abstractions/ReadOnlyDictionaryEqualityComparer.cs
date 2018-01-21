using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Google.Protobuf.Reflection;

namespace MicroPipes
{
    public class ReadOnlyDictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IReadOnlyDictionary<TKey, TValue>>
    {
        public bool Equals(IReadOnlyDictionary<TKey, TValue> x, IReadOnlyDictionary<TKey, TValue> y)
        {
            IEqualityComparer<TValue> valueComparer;
            switch (x)
            {
                case ImmutableDictionary<TKey, TValue> id:
                    valueComparer = id.ValueComparer;
                    break;
                default:
                    valueComparer = EqualityComparer<TValue>.Default;
                    break;
            }

            if (ReferenceEquals(x, y)) return true;
            if (x == null) return false;
            if (y == null) return false;
            return x.Count == y.Count && x.All(p => y.TryGetValue(p.Key, out var v) && valueComparer.Equals(p.Value, v));
        }

        public int GetHashCode(IReadOnlyDictionary<TKey, TValue> obj)
        {
            IEqualityComparer<TKey> keyComparer;
            IEqualityComparer<TValue> valueComparer;
            switch (obj)
            {
                case ImmutableDictionary<TKey, TValue> id:
                    valueComparer = id.ValueComparer;
                    keyComparer = id.KeyComparer;
                    break;
                case Dictionary<TKey, TValue> rod:
                    keyComparer = rod.Comparer;
                    valueComparer = EqualityComparer<TValue>.Default;
                    break;
                default:
                    valueComparer = EqualityComparer<TValue>.Default;
                    keyComparer = EqualityComparer<TKey>.Default;
                    break;
            }
            
            return obj?.Aggregate(0, (a, p) => {
                unchecked
                {
                    return (a * 397 * 397) ^ (keyComparer.GetHashCode(p.Key) * 397) ^ valueComparer.GetHashCode(p.Value);
                } 
            }) ?? 0;
        }
        
        public static readonly ReadOnlyDictionaryEqualityComparer<TKey, TValue> Default = new ReadOnlyDictionaryEqualityComparer<TKey, TValue>();
    }
    
    
}