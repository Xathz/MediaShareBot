using System;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Extensions;
using MediaShareBot.Settings;
using Newtonsoft.Json.Linq;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public static class DonationEvent {

        public static async void Process(JObject eventObject) {

            string from = eventObject.FindValueByKey<string>("from").SanitizeForMarkdown();
            string fromId = eventObject.FindValueByKey<string>("from_user_id");

            string message = eventObject.FindValueByKey<string>("message").SanitizeForMarkdown();
            string formattedMessage = !string.IsNullOrWhiteSpace(message) ? $"```{message}```" : "";

            decimal amount = eventObject.FindValueByKey<decimal>("amount");
            string formattedAmount = eventObject.FindValueByKey<string>("formattedAmount");
            string icon = amount >= SettingsManager.Configuration.LargeDonationThreshold ? ":small_blue_diamond: " : "";

            // Donation message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{from}** donated **{formattedAmount}**{formattedMessage}");

            string mediaId = eventObject.FindValueByParentAndKey<string>("media", "id");
            string mediaTitle = eventObject.FindValueByParentAndKey<string>("media", "title").SanitizeForMarkdown();
            string mediaStartTime = eventObject.FindValueByParentAndKey<string>("media", "start_time", "0");

            // Event log
            await DiscordClient.SendEventLogMessageAsync($"Streamlabs Donation```{from} ({fromId}){Environment.NewLine}" +
                $"{formattedAmount}{Environment.NewLine}" +
                $"{(!string.IsNullOrWhiteSpace(message) ? message : "<no message>")}{Environment.NewLine}{Environment.NewLine}" +
                $"{(!string.IsNullOrWhiteSpace(mediaTitle) ? mediaTitle : "<no media>")}{Environment.NewLine}" +
                $"{(!string.IsNullOrEmpty(mediaId) ? $"https://www.youtube.com/watch?v={mediaId}&t={mediaStartTime}" : "<no media>")}{Environment.NewLine}{Environment.NewLine}" +
                $" id {eventObject.FindValueByKey<string>("id")}{Environment.NewLine}" +
                $"_id {eventObject.FindValueByKey<string>("_id")}");

        }

        public static string GetBestThumbnailUrl(JObject eventObject) {
            // 1280x720
            string maxRes = eventObject.FindValueByParentAndKey<string>("maxres", "url");
            if (!string.IsNullOrEmpty(maxRes)) { return maxRes; }

            // 640x480
            string standardRes = eventObject.FindValueByParentAndKey<string>("standard", "url");
            if (!string.IsNullOrEmpty(standardRes)) { return standardRes; }

            // 480x360
            string highRes = eventObject.FindValueByParentAndKey<string>("high", "url");
            if (!string.IsNullOrEmpty(highRes)) { return highRes; }

            // 320x180
            string mediumRes = eventObject.FindValueByParentAndKey<string>("medium", "url");
            if (!string.IsNullOrEmpty(mediumRes)) { return mediumRes; }

            // 120x90
            string defaultRes = eventObject.FindValueByParentAndKey<string>("default", "url");
            if (!string.IsNullOrEmpty(defaultRes)) { return defaultRes; }

            return Constants.YouTubeLogoUrl;
        }

    }

}
