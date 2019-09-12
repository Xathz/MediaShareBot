using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MediaShareBot.Extensions {

    public static class EnumerableExt {

        /// <summary>
        /// Pick a random entry from a list.
        /// </summary>
        public static T PickRandom<T>(this IEnumerable<T> source) => source.PickRandom(1).Single();

        /// <summary>
        /// Pick mutiple random entries from a list.
        /// </summary>
        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) => source.Shuffle().Take(count);

        /// <summary>
        /// Shuffle a list.
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.OrderBy(x => Guid.NewGuid());

        /// <summary>
        /// Convert a dictionary to concurrent dictionary.
        /// </summary>
        /// <remarks>https://stackoverflow.com/a/12396386</remarks>
        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TKey, TValue, TElement>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TElement> elementSelector) =>
                        new ConcurrentDictionary<TKey, TElement>(source.Select(x => new KeyValuePair<TKey, TElement>(keySelector(x), elementSelector(x))));

        /// <summary>
        /// Convert a dictionary to concurrent dictionary with a key comparer.
        /// </summary>
        /// <remarks>https://stackoverflow.com/a/12396386</remarks>
        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TKey, TValue, TElement>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TElement> elementSelector, IEqualityComparer<TKey> comparer) =>
                        new ConcurrentDictionary<TKey, TElement>(source.Select(x => new KeyValuePair<TKey, TElement>(keySelector(x), elementSelector(x))), comparer);

    }

}
