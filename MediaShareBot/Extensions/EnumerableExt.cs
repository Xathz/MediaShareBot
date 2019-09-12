using System;
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

    }

}
