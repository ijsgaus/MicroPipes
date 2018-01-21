using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MicroPipes
{
    public static class Extensions
    {
        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValues<TKey, TValue>(
            this IEnumerable<(TKey, TValue)> enumerable)
            => enumerable.Select(p => new KeyValuePair<TKey, TValue>(p.Item1, p.Item2));

        public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(
            this IEnumerable<(TKey, TValue)> enumerable, bool throwOnNull = false)
        {
            if(throwOnNull && enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            return enumerable == null
                ? ImmutableDictionary<TKey, TValue>.Empty
                : ImmutableDictionary.CreateRange(enumerable.ToKeyValues());

        }
        
        public static bool DefaultComparerEqual<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary, IReadOnlyDictionary<TKey, TValue> other)
            => ReadOnlyDictionaryEqualityComparer<TKey, TValue>.Default.Equals(dictionary, other);
        
        public static int DefaultComparerHashCode<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary)
            => ReadOnlyDictionaryEqualityComparer<TKey, TValue>.Default.GetHashCode(dictionary);
        
        public static bool DefaultComparerEqual<T>(
            this IReadOnlyCollection<T> dictionary, IReadOnlyCollection<T> other)
            => ReadOnlyCollectionEqualityComparer<T>.Default.Equals(dictionary, other);
        
        public static int DefaultComparerHashCode<T>(
            this IReadOnlyCollection<T> dictionary)
            => ReadOnlyCollectionEqualityComparer<T>.Default.GetHashCode(dictionary);
    }
}