using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Settings;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class SubscriptionEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public SubscriptionEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            if (Parser.IsSubscriptionGift) { return; }

            string icon = Parser.Months >= SettingsManager.Configuration.LargeSubMonthHolder ? ":small_orange_diamond: " : "";
            string monthWord = Parser.Months > 1 ? "months" : "month";

            // Subscription message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{Parser.FromUser}** subscribed for **{Parser.Months} {monthWord}** ({Parser.SubscriptionPlan}){Parser.MessageFormatted}");

            // Event log
            await DiscordClient.SendEventLogMessageAsync("Twitch Subscription",
                $"{Parser.FromUser}",
                $"{Parser.Months} {monthWord}",
                $"{(!string.IsNullOrEmpty(Parser.Message) ? Parser.Message : "<no message>")}",
                "",
                $"_id {Parser.EventLogUnderscoreId}");

        }

    }

}
