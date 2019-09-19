using System.Threading.Tasks;
using MediaShareBot.Clients.Discord;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public class MediaShareEvent : IStreamlabsEvent {

        public EventValueParser Parser { get; private set; }

        public MediaShareEvent(EventValueParser parser) => Parser = parser;

        public async Task Process() {

            // Media share message
            await DiscordClient.SendMediaShareMessageAsync(Parser.FromUser, Parser.Message, Parser.AmountFormatted, Parser.DateTime,
                Parser.MediaThumbnailUrl, Parser.MediaUrl, Parser.MediaViews, Parser.MediaTitle, Parser.MediaChannelUrl, Parser.MediaChannelTitle);

            // Event log
            await DiscordClient.SendEventLogMessageAsync("Streamlabs Donation",
                $"{Parser.FromUser}",
                $"{Parser.AmountFormatted}",
                $"{(!string.IsNullOrEmpty(Parser.Message) ? Parser.Message : "<no message>")}",
                "",
                $"{Parser.MediaTitle}",
                $"{Parser.MediaUrl}",
                "",
                $" id {Parser.EventLogId}",
                $"_id {Parser.EventLogUnderscoreId}");

        }

    }

}
