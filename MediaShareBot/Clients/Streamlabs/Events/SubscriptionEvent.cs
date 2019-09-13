using System;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Extensions;
using MediaShareBot.Settings;
using Newtonsoft.Json.Linq;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public static class SubscriptionEvent {

        public static async void Process(JObject eventObject) {

            string type = eventObject.FindValueByKey<string>("sub_type");
            if (type == "subgift") { return; }

            string displayName = eventObject.FindValueByKey<string>("display_name").SanitizeForMarkdown();

            string message = eventObject.FindValueByKey<string>("message").SanitizeForMarkdown();
            string formattedMessage = !string.IsNullOrWhiteSpace(message) ? $"```{message}```" : "";
            string months = eventObject.FindValueByKey<string>("months");

            string icon = "";
            string monthWord = "month";
            if (int.TryParse(months, out int intMonths)) {
                icon = intMonths >= SettingsManager.Configuration.LargeSubMonthHolder ? ":small_orange_diamond: " : icon;
                monthWord = intMonths > 1 ? "months" : monthWord;
            }

            string planName = "";
            if (Cache.TwitchSubscriptionPlans.TryGetValue(eventObject.FindValueByKey<string>("sub_plan"), out string planLookup)) {
                planName = $" ({planLookup})";
            }

            // Subscription message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{displayName}** subscribed for **{months} {monthWord}**{planName}{formattedMessage}");

            // Event log
            await DiscordClient.SendEventLogMessageAsync($"Twitch Subscription```{displayName}{Environment.NewLine}" +
                $"{months} {monthWord}{Environment.NewLine}" +
                $"{(!string.IsNullOrWhiteSpace(message) ? message : "<no message>")}{Environment.NewLine}{Environment.NewLine}" +
                $" id {eventObject.FindValueByKey<string>("id")}{Environment.NewLine}" +
                $"_id {eventObject.FindValueByKey<string>("_id")}");

        }

    }

}
