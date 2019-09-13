using System;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Extensions;
using MediaShareBot.Settings;
using Newtonsoft.Json.Linq;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public static class BitDonationEvent {

        public static async void Process(JObject eventObject) {

            string displayName = eventObject.FindValueByKey<string>("display_name").SanitizeForMarkdown();

            string message = eventObject.FindValueByKey<string>("message").SanitizeForMarkdown();
            string cheerlessMessage = message.RemoveCheermotes();
            string formattedMessage = !string.IsNullOrWhiteSpace(cheerlessMessage) ? $"```{cheerlessMessage}```" : "";

            int amount = eventObject.FindValueByKey<int>("amount");
            string icon = amount >= SettingsManager.Configuration.LargeBitsDonationThreshold ? ":small_blue_diamond: " : "";

            // Bits donation message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{displayName}** donated **{amount} bits**{formattedMessage}");

            // Event log
            await DiscordClient.SendEventLogMessageAsync($"Twitch Bits Donation```{displayName}{Environment.NewLine}" +
                $"{amount} bits{Environment.NewLine}" +
                $"{(!string.IsNullOrWhiteSpace(message) ? message : "<no message>")}{Environment.NewLine}{Environment.NewLine}" +
                $" id {eventObject.FindValueByKey<string>("id")}{Environment.NewLine}" +
                $"_id {eventObject.FindValueByKey<string>("_id")}");

        }

    }

}
