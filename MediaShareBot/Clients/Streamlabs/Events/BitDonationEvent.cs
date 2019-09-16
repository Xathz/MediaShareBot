using System;
using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Settings;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class BitDonationEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public BitDonationEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            string icon = Parser.Amount >= SettingsManager.Configuration.LargeBitsDonationThreshold ? ":small_blue_diamond: " : "";

            // Bits donation message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{Parser.FromUser}** donated **{Parser.Amount} bits**{Parser.MessageFormatted}");

            // Event log
            await DiscordClient.SendEventLogMessageAsync($"Twitch Bits Donation```{Parser.FromUser}{Environment.NewLine}" +
                $"{Parser.Amount} bits{Environment.NewLine}" +
                $"{(!string.IsNullOrWhiteSpace(Parser.Message) ? Parser.Message : "<no message>")}{Environment.NewLine}{Environment.NewLine}" +
                $" id {Parser.EventLogId}{Environment.NewLine}" +
                $"_id {Parser.EventLogUnderscoreId}```");

        }

    }

}
