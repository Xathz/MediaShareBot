using System;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Extensions;
using MediaShareBot.Settings;
using Newtonsoft.Json.Linq;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public static class SubscriptionGiftEvent {

        public static async void Process(JObject eventObject) {

            string displayName = eventObject.FindValueByKey<string>("gifter_display_name").SanitizeForMarkdown();

            string amount = eventObject.FindValueByKey<string>("amount");

            string icon = "";
            string subWord = "sub";
            if (int.TryParse(amount, out int intAmount)) {
                icon = intAmount >= SettingsManager.Configuration.LargeSubGiftThreshold ? ":small_orange_diamond: " : icon;
                subWord = intAmount > 1 ? "subs" : subWord;
            }

            string planName = "";
            if (Cache.TwitchSubscriptionPlans.TryGetValue(eventObject.FindValueByKey<string>("sub_plan"), out string planLookup)) {
                planName = $" ({planLookup})";
            }

            // Subscription gift messages get spammed from the socket mutiple times
            if (HashTracker.Add(Enums.EventType.SubscriptionGift, displayName, eventObject.FindValueByKey<string>("_id"))) {

                // Subscription gift message
                await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{displayName}** gifted **{amount} {subWord}**{planName}");

                // Event log
                await DiscordClient.SendEventLogMessageAsync($"Twitch Subscription Gift```{displayName}{Environment.NewLine}" +
                    $"{amount} {subWord}{Environment.NewLine}{Environment.NewLine}" +
                    $" id {eventObject.FindValueByKey<string>("id")}{Environment.NewLine}" +
                    $"_id {eventObject.FindValueByKey<string>("_id")}");

            }

        }

    }

}
