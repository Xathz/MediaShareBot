namespace MediaShareBot.Settings {

    public class DiscordChannels {

        /// <summary>
        /// Channel to send event log, status, and various Streamlabs and Twitch logs to.
        /// Set to '0' to disable these messages.
        /// </summary>
        public ulong EventLog { get; set; } = 0;

        /// <summary>
        /// Channel to send the media share videos to.
        /// Set to '0' to disable these messages.
        /// </summary>
        public ulong MediaShare { get; set; } = 0;

        /// <summary>
        /// Channel to send subscription, gifted subscriptions, donations, and bit donations messages to.
        /// Set to '0' to disable these messages.
        /// </summary>
        public ulong SubsAndDonations { get; set; } = 0;

    }

}
