using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Humanizer;
using MediaShareBot.Settings;

namespace MediaShareBot.Clients.Discord {

    public static partial class DiscordClient {

        public static async Task SendEventLogMessageAsync(string title, params string[] message) {
            string fullMessage = string.Join(Environment.NewLine, message);

            if (SettingsManager.Configuration.DiscordChannels.EventLog == 0) { return; }
            if (!SentMessagesCache.Add($"{title} {fullMessage}")) { return; }

            try {
                if (_DiscordClient.GetChannel(SettingsManager.Configuration.DiscordChannels.EventLog) is IMessageChannel channel) {
                    await channel.SendMessageAsync($"● {title}```{Environment.NewLine}{fullMessage}```");
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        public static async Task SendMediaShareMessageAsync(string from, string message, string amount, DateTime donatedAgo,
            string mediaThumbnailUrl, string mediaUrl, int mediaViews, string mediaTitle, string mediaChannelUrl, string mediaChannelTitle) {

            if (SettingsManager.Configuration.DiscordChannels.MediaShare == 0) { return; }
            if (!SentMessagesCache.Add(from, message, amount)) { return; }

            try {
                if (_DiscordClient.GetChannel(SettingsManager.Configuration.DiscordChannels.MediaShare) is IMessageChannel channel) {

                    EmbedBuilder builder = new EmbedBuilder() {
                        Color = new Color(Constants.YouTubeColor.R, Constants.YouTubeColor.G, Constants.YouTubeColor.B),
                        ImageUrl = mediaThumbnailUrl,
                        Url = mediaUrl,
                        Title = mediaTitle
                    };

                    builder.Author = new EmbedAuthorBuilder() {
                        Url = mediaChannelUrl,
                        Name = mediaChannelTitle
                    };

                    builder.AddField("Donor / Amount", $"[{from}](https://www.twitch.tv/{from}, \"If people are allowed to enter any username when they donate, this Twitch link may not be accurate.{Environment.NewLine}" +
                        $"Clicking this link will take you to: https://www.twitch.tv/{from}\") / {amount}", true);

                    builder.AddField("Views", mediaViews.ToString("N0", CultureInfo.InvariantCulture), true);

                    if (!string.IsNullOrWhiteSpace(message)) {
                        builder.AddField("Message", message);
                    }

                    builder.Footer = new EmbedFooterBuilder() {
                        Text = $"Donated {DateTime.UtcNow.Subtract(donatedAgo).Humanize(2)} ago"
                    };

                    await channel.SendMessageAsync(embed: builder.Build());
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        public static async Task SendSubOrDonationMessageAsync(string message) {
            if (SettingsManager.Configuration.DiscordChannels.SubsAndDonations == 0) { return; }
            if (!SentMessagesCache.Add(message)) { return; }

            try {
                if (_DiscordClient.GetChannel(SettingsManager.Configuration.DiscordChannels.SubsAndDonations) is IMessageChannel channel) {
                    await channel.SendMessageAsync(message);
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

    }

}
