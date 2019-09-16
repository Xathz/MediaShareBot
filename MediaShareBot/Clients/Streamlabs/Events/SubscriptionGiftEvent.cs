using System;
using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Settings;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class SubscriptionGiftEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public SubscriptionGiftEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            string icon = Parser.Amount >= SettingsManager.Configuration.LargeSubGiftThreshold ? ":small_orange_diamond: " : "";
            string subWord = Parser.Amount > 1 ? "subs" : "sub";

            // Subscription gift message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{Parser.FromUser}** gifted **{Parser.Amount} {subWord}** ({Parser.SubscriptionPlan})");

            // Event log
            await DiscordClient.SendEventLogMessageAsync($"Twitch Subscription Gift```{Parser.FromUser}{Environment.NewLine}" +
                $"{Parser.Amount} {subWord}{Environment.NewLine}{Environment.NewLine}" +
                $" id {Parser.EventLogId}{Environment.NewLine}" +
                $"_id {Parser.EventLogUnderscoreId}```");

        }

    }

}
