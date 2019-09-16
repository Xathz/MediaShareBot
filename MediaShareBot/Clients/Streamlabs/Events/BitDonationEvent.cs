using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Settings;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class BitDonationEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public BitDonationEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            string icon = Parser.Amount >= SettingsManager.Configuration.LargeBitsDonationThreshold ? ":small_blue_diamond: " : "";
            string bitWord = Parser.Amount > 1 ? "bits" : "bit";

            // Bits donation message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{Parser.FromUser}** donated **{Parser.Amount} {bitWord}* *{Parser.MessageFormatted}");

            // Event log
            await DiscordClient.SendEventLogMessageAsync("Twitch Bits Donation", $"{Parser.FromUser}",
                $"{Parser.Amount} {bitWord}",
                $"{(!string.IsNullOrEmpty(Parser.Message) ? Parser.Message : "<no message>")}",
                "",
                $" id {Parser.EventLogId}",
                $"_id {Parser.EventLogUnderscoreId}");

        }

    }

}
