using System;
using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Settings;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class DonationEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public DonationEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            string icon = Parser.Amount >= SettingsManager.Configuration.LargeDonationThreshold ? ":small_blue_diamond: " : "";

            // Donation message
            await DiscordClient.SendSubOrDonationMessageAsync($"{icon}**{Parser.FromUser}** donated **{Parser.AmountFormatted}**{Parser.MessageFormatted}");

            // Event log
            await DiscordClient.SendEventLogMessageAsync($"Streamlabs Donation```{Parser.FromUser} ({Parser.FromUserId}){Environment.NewLine}" +
                $"{Parser.AmountFormatted}{Environment.NewLine}" +
                $"{(!string.IsNullOrWhiteSpace(Parser.Message) ? Parser.Message : "<no message>")}{Environment.NewLine}{Environment.NewLine}" +
                $"{(Parser.IsMediaDonation ? Parser.MediaTitle : "<no media>")}{Environment.NewLine}" +
                $"{(Parser.IsMediaDonation ? $"https://www.youtube.com/watch?v={Parser.MediaId}&t={Parser.MediaStartTime}" : "<no media>")}{Environment.NewLine}{Environment.NewLine}" +
                $" id {Parser.EventLogId}{Environment.NewLine}" +
                $"_id {Parser.EventLogUnderscoreId}```");

        }

    }

}
