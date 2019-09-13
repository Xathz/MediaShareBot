using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MediaShareBot.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace MediaShareBot.Clients.Discord {

    public static partial class DiscordClient {

        public static async Task SendEventLogMessageAsync(string message) {
            if (SettingsManager.Configuration.DiscordChannels.EventLog == 0) { return; }

            try {
                if (_DiscordClient.GetChannel(SettingsManager.Configuration.DiscordChannels.EventLog) is IMessageChannel channel) {
                    await channel.SendMessageAsync(message);
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        public static async Task SendMediaShareMessageAsync(string from, string message, string amount, string donatedAgo,
            string mediaThumbnailUrl, string mediaUrl, string mediaViews, string mediaTitle,
            string mediaChannelUrl, string mediaChannelTitle) {

            if (SettingsManager.Configuration.DiscordChannels.MediaShare == 0) { return; }

            try {
                if (_DiscordClient.GetChannel(SettingsManager.Configuration.DiscordChannels.MediaShare) is IMessageChannel channel) {

                    EmbedBuilder builder = new EmbedBuilder() {
                        Color = new Color(Constants.StreamlabsColor.R, Constants.StreamlabsColor.G, Constants.StreamlabsColor.B),
                        ThumbnailUrl = mediaThumbnailUrl,
                        Url = mediaUrl,
                        Title = mediaTitle
                    };

                    builder.Author = new EmbedAuthorBuilder() {
                        Url = mediaChannelUrl,
                        Name = mediaChannelTitle
                    };

                    builder.AddField("Donation / Amount", $"[{from}(https://www.twitch.tv/{from}], If people are allowed to enter any username when they donate, this Twitch link may not be accurate.{Environment.NewLine}" +
                        $"Clicking this link will take you to: https://www.twitch.tv/${from}) / {amount}", true);

                    builder.AddField("Views", mediaViews, true);

                    builder.AddField("Message", message);

                    builder.Footer = new EmbedFooterBuilder() {
                        Text = $"Donated {donatedAgo}"
                    };

                    await channel.SendMessageAsync(embed: builder.Build());
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        public static async Task SendSubOrDonationMessageAsync(string message) {
            if (SettingsManager.Configuration.DiscordChannels.SubsAndDonations == 0) { return; }

            try {
                if (_DiscordClient.GetChannel(SettingsManager.Configuration.DiscordChannels.SubsAndDonations) is IMessageChannel channel) {
                    await channel.SendMessageAsync(message);
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        /// <summary>
        /// Send a message to a channel or user (direct message).
        /// </summary>
        /// <param name="id">Channel id or user id (direct message) to send a message to.</param>
        /// <param name="message">Message to send.</param>
        /// <returns>Id of the message if successful.</returns>
        public static async Task<ulong?> SendMessageAsync(ulong id, string message) {
            try {
                if (_DiscordClient.GetChannel(id) is IMessageChannel channel) {
                    return (await channel.SendMessageAsync(message)).Id;
                }

                if (_DiscordClient.GetUser(id) is IUser user) {
                    return (await user.SendMessageAsync(message)).Id;
                }

                return null;
            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
                return null;
            }
        }

    }

}
