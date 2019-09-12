using System;
using MediaShareBot.Utilities;
using static MediaShareBot.Clients.Streamlabs.Enums;

namespace MediaShareBot.Clients.Streamlabs {

    public static class HashTracker {

        private static readonly ConcurrentHashSet<string> _ProcessedHashes = new ConcurrentHashSet<string>();

        public static string GenerateHash(EventType eventType, string from, string message) => $"{eventType.ToString()}:{from.ToLowerInvariant()}:{message.ToLowerInvariant()}:{DateTime.UtcNow.ToString("yyyyMMddHHmm")}";

        /// <summary>
        /// Add a hash. Returns <see langword="true"/> if added; <see langword="false"/> if already exists.
        /// </summary>
        public static bool Add(EventType eventType, string from, string message) => _ProcessedHashes.Add(GenerateHash(eventType, from, message));

    }

}
