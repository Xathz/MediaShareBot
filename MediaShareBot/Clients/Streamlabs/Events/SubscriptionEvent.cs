using System;
using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Settings;
using static MediaShareBot.Clients.Streamlabs.Enums;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class SubscriptionEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public SubscriptionEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            if (Parser.Type == EventType.SubscriptionGift) { return; }

            string icon = Parser.Months >= SettingsManager.Configuration.LargeSubMonthHolder ? ":small_orange_diamond: " : "";
            string monthWord = Parser.Months > 1 ? "months" : "month";
       
            // Subscription message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{Parser.From}** subscribed for **{Parser.Months} {monthWord}** ({Parser.SubscriptionPlan}){Parser.MessageCodeBlock}");

            // Event log
            await DiscordClient.SendEventLogMessageAsync($"Twitch Subscription```{Parser.From}{Environment.NewLine}" +
                $"{Parser.Months} {monthWord}{Environment.NewLine}" +
                $"{(!string.IsNullOrWhiteSpace(Parser.Message) ? Parser.Message : "<no message>")}{Environment.NewLine}{Environment.NewLine}" +
                $" id {Parser.EventLogId}{Environment.NewLine}" +
                $"_id {Parser.EventLogUnderscoreId}```");

        }

    }

}
