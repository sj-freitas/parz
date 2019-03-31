using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            var idx = 0;

            foreach (var curr in enumerable)
            {
                if (predicate(curr))
                {
                    return idx;
                }

                idx++;
            }

            return -1;
        }

        public static int LastIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            var currIdxOfLast = -1;
            var idx = 0;
            foreach (var curr in enumerable)
            {
                if (predicate(curr))
                {
                    currIdxOfLast = idx;
                }

                idx++;
            }

            return currIdxOfLast;
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> AddEntry<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> entries, TKey key, TValue value)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            return entries.Concat(new[] { new KeyValuePair<TKey, TValue>(key, value) });
        }

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> entries, 
            IEqualityComparer<TKey> keyComparer)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }
            if (keyComparer == null)
            {
                throw new ArgumentNullException(nameof(keyComparer));
            }

            return entries
                .ToDictionary(t => t.Key, t => t.Value, keyComparer);
        }
    }
}
