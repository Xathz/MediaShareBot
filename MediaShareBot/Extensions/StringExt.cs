using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MediaShareBot.Extensions {

    public static class StringExt {

        private static readonly Regex _UrlRegex = new Regex("https?://", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static Regex _CheermotesRegex = null;

        private static readonly string[] _MarkdownSensitiveCharacters = { "\\", "*", "_", "~", "`", "|", ">" };

        /// <summary>
        /// Call once the cheermotes are downloaded and or parsed.
        /// </summary>
        public static void SetCheermotesRegex() => _CheermotesRegex = new Regex($@"({string.Join("|", Cache.TwitchCheermotes)})\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Split a string into chunks based on max length.
        /// </summary>
        public static List<string> SplitIntoChunks(this string input, int maxChunkSize) {
            List<string> chunks = new List<string>();
            if (string.IsNullOrWhiteSpace(input)) { return chunks; }

            string[] words = input.Split(' ');
            int index = 0;

            foreach (string word in words) {
                if (chunks.Count == index) {
                    chunks.Add("");
                }

                if ((chunks[index].Length + word.Length) <= maxChunkSize) {
                    chunks[index] += $" {word}";

                } else {
                    chunks.Add($" {word}");
                    index++;
                }
            }

            return chunks.Select(x => x.Trim()).ToList();
        }

        /// <summary>
        /// Split a string into chunks based on max length, preserving and not breaking words or before a new line. 
        /// </summary>
        public static List<string> SplitIntoChunksPreserveNewLines(this string input, int maxChunkSize) {
            List<string> chunks = new List<string>();
            if (string.IsNullOrWhiteSpace(input)) { return chunks; }

            string[] lines = input.Split(new string[] { Environment.NewLine, "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;

            foreach (string line in lines) {
                string currentLine = $"{line}{Environment.NewLine}";

                if (currentLine.Length > maxChunkSize) {
                    throw new OverflowException("A single currentLine is longer than maxChunkSize.");
                }

                if (chunks.Count == index) {
                    chunks.Add("");
                }

                if ((chunks[index].Length + currentLine.Length) <= maxChunkSize) {
                    chunks[index] += currentLine;

                } else {
                    chunks.Add(currentLine);
                    index++;
                }
            }

            return chunks.Select(x => x.Trim()).ToList();
        }

        /// <summary>
        /// Extract a string between two strings.
        /// </summary>
        public static string Extract(this string input, string start, string end) {
            int from = input.IndexOf(start) + start.Length;
            int to = input.LastIndexOf(end);

            return input.Substring(from, to - from);
        }

        /// <summary>
        /// Remove all empty lines.
        /// </summary>
        public static string RemoveEmptyLines(this string input) {
            List<string> clean = input.SplitByNewLines();
            return string.Join(Environment.NewLine, clean);
        }

        /// <summary>
        /// Remove all new lines and line breaks from a string.
        /// </summary>
        public static string RemoveNewLines(this string input) => input.Replace(Environment.NewLine, " ").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");

        /// <summary>
        /// Split a string by all possible new line and return characters.
        /// </summary>
        public static List<string> SplitByNewLines(this string input) => input.Split(new string[] { Environment.NewLine, "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();

        /// <summary>
        /// Split a string by spaces.
        /// </summary>
        public static List<string> SplitBySpace(this string input) => input.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        /// <summary>
        /// Count occurrences of a pattern.
        /// </summary>
        /// <remarks>https://www.dotnetperls.com/string-occurrence</remarks>
        public static int CountStringOccurrences(this string input, string pattern) {
            int count = 0;
            int i = 0;
            while ((i = input.IndexOf(pattern, i)) != -1) {
                i += pattern.Length;
                count++;
            }

            return count;
        }

        /// <summary>
        /// Returns a new string that centers the characters in this string by padding them with spaces on the left and right, for a specified total length.
        /// </summary>
        /// <param name="totalWidth">The number of characters in the resulting string, equal to the number of original characters plus any additional padding characters.</param>
        /// <remarks>https://stackoverflow.com/a/17590723</remarks>
        public static string PadBoth(this string input, int totalWidth) {
            if (string.IsNullOrWhiteSpace(input)) { return input; }

            int spaces = totalWidth - input.Length;
            int padLeft = spaces / 2 + input.Length;
            return input.PadLeft(padLeft).PadRight(totalWidth);
        }

        /// <summary>
        /// Check if string contains at least 1 url.
        /// </summary>
        public static bool ContainsUrls(this string input) {
            MatchCollection matches = _UrlRegex.Matches(input);
            return (matches.Count > 0) ? true : false;
        }

        /// <summary>
        /// Get all urls from a string.
        /// </summary>
        public static List<string> GetUrls(this string input) {
            if (string.IsNullOrWhiteSpace(input)) { return new List<string>(); }

            MatchCollection matches = _UrlRegex.Matches(input);
            List<string> urls = new List<string>();

            foreach (Match match in matches) {
                urls.Add(match.Value);
            }

            return urls;
        }

        /// <summary>
        /// Remove all Twitch cheermotes from a string.
        /// </summary>
        public static string RemoveCheermotes(this string input) {
            if (_CheermotesRegex == null) { return input; }
            if (string.IsNullOrWhiteSpace(input)) { return input; }

            MatchCollection matches = _CheermotesRegex.Matches(input);
            foreach (Match match in matches) {
                input = input.Replace(match.Value, "");
            }

            return input.Trim();
        }

        /// <summary>
        /// Sanitizes the string, safely escaping any markdown sequences.
        /// </summary>
        /// <remarks>https://git.io/JeY0Y</remarks>
        public static string SanitizeForMarkdown(this string input) {
            if (string.IsNullOrWhiteSpace(input)) { return input; }

            foreach (string unsafeChar in _MarkdownSensitiveCharacters) {
                input = input.Replace(unsafeChar, $"\\{unsafeChar}");
            }

            return input.Trim();
        }

    }

}
