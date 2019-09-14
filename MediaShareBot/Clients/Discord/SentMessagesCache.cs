using System;
using System.Runtime.Caching;

namespace MediaShareBot.Clients.Discord {

    public static class SentMessagesCache {

        private static readonly MemoryCache _Cache = MemoryCache.Default;

        private const string _Prefix = "sentmessage";
        private const byte _Object = 0; // Cache objects can't be null

        /// <summary>
        /// Add a message hash.
        /// </summary>
        /// <param name="expirationMin">Number of minutes the cache will live. If <see langword="null"/> it will never expire.</param>
        /// <returns><see langword="true"/> if added; <see langword="false"/> if already exists.</returns>
        public static bool Add(string message, int? expirationMin = 5) {
            if (string.IsNullOrWhiteSpace(message)) { return false; }

            message = message.ToLowerInvariant();

            if (expirationMin.HasValue) {
                return _Cache.Add($"{_Prefix}_{message}", _Object, DateTimeOffset.UtcNow.AddMinutes(expirationMin.Value));
            } else {
                return _Cache.Add($"{_Prefix}_{message}", _Object, ObjectCache.InfiniteAbsoluteExpiration);
            }
        }

        /// <summary>
        /// Add a message hash.
        /// </summary>
        /// <param name="expirationMin">Number of minutes the cache will live. If <see langword="null"/> it will never expire.</param>
        /// <returns><see langword="true"/> if added; <see langword="false"/> if already exists.</returns>
        public static bool Add(string from, string message, string amount, int? expirationMin = 5) => Add($"{from}:{message}:{amount}", expirationMin);

        /// <summary>
        /// Add a message hash.
        /// </summary>
        /// <param name="expirationMin">Number of minutes the cache will live. If <see langword="null"/> it will never expire.</param>
        /// <returns><see langword="true"/> if added; <see langword="false"/> if already exists.</returns>
        public static bool Add(string eventType, string from, string message, string amount, int? expirationMin = 5) => Add($"{eventType}:{from}:{message}:{amount}", expirationMin);

    }

}
