namespace MediaShareBot.Settings {

    public class Configuration {

        /// <summary>
        /// Bot nickname.
        /// </summary>
        public string BotNickname { get; set; } = "MediaShare";

        /// <summary>
        /// What "game" is the bot playing.
        /// </summary>
        public string BotPlaying { get; set; } = "Posting links from media share";

        /// <summary>
        /// Discord api token.
        /// </summary>
        public string DiscordToken { get; set; } = string.Empty;

        /// <summary>
        /// Streamlabs api token.
        /// </summary>
        public string StreamlabsToken { get; set; } = string.Empty;

        /// <summary>
        /// Threshold to consider a "large donation".
        /// Threshold is equal to or greater than this value.
        /// </summary>
        public int LargeDonationThreshold { get; set; } = 20;

        /// <summary>
        /// Threshold to consider a "large bits donation".
        /// Threshold is equal to or greater than this value.
        /// 1 bit = $0.01
        /// </summary>
        public int LargeBitsDonationThreshold { get; set; } = 2000;

        /// <summary>
        /// Threshold to consider a "large subscription gift".
        /// Threshold is equal to or greater than this value.
        /// </summary>
        public int LargeSubGiftThreshold { get; set; } = 10;

        /// <summary>
        /// Threshold to consider a "long subscription holder".
        /// Threshold is equal to or greater than this value.
        /// </summary>
        public int LargeSubMonthHolder { get; set; } = 12;

        /// <summary>
        /// Threshold to consider a "large raid".
        /// Threshold is equal to or greater than this value.
        /// </summary>
        public int LargeRaid { get; set; } = 40;

        /// <summary>
        /// Discord channels
        /// </summary>
        public DiscordChannels DiscordChannels { get; set; } = new DiscordChannels();

    }

}
