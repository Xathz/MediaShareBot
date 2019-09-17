using MediaShareBot.Exceptions;

namespace MediaShareBot.Clients.Streamlabs {

    public static class Enums {

        public static EventType ParseEventType(string eventType) {
            if (string.IsNullOrEmpty(eventType)) { return EventType.Default; }

            switch (eventType.ToLowerInvariant()) {
                // Streamlabs alerts
                case "alertplaying": return EventType.AlertPlaying;
                case "acceptalert": return EventType.AcceptAlert;
                case "skipalert": return EventType.SkipAlert;

                // Donations
                case "donation": return EventType.Donation;
                case "donationgoalstart": return EventType.DonationGoalStart;
                case "donationgoalend": return EventType.DonationGoalEnd;
                case "bits": return EventType.BitDonation;

                // Subscriptions
                case "subscription": return EventType.Subscription;
                case "resub": return EventType.ReSubscription;
                case "submysterygift": return EventType.SubscriptionGift;
                case "subtember": return EventType.Subtember;

                // Twitch other
                case "follow": return EventType.Follow;
                case "raid": return EventType.Raid;
                case "host": return EventType.Host;

                // Streamlabs media share
                case "mediashareevent": return EventType.MediaShare;
                case "setvolume": return EventType.SetVolume;
                case "mutevolume": return EventType.MuteVolume;
                case "unmutevolume": return EventType.UnmuteVolume;
                case "pausequeue": return EventType.PauseQueue;
                case "unpausequeue": return EventType.ResumeQueue;
                case "hidevideo": return EventType.HideVideo;
                case "showvideo": return EventType.ShowVideo;

                // Streamlabs system events
                case "recenteventsevent": return EventType.RecentEventsEvent;
                case "sessionreset": return EventType.SessionReset;
                case "streamlabels": return EventType.Streamlabels;
                case "streamlabels.underlying": return EventType.StreamlabelsUnderlying;
                case "chatboxsettingsupdate": return EventType.ChatBoxSettingsUpdate;
                case "eventspanelsettingsupdate": return EventType.EventsPanelSettingsUpdate;
                case "separatedlayout": return EventType.SeparatedLayout;

                default: throw new StreamlabsParseTypeException($"Failed to parse type. Default reached. Supplied string: {eventType}");
            }
        }

        public enum EventType {
            Default = 0, // Better than nullable

            // Streamlabs alerts
            AlertPlaying,
            AcceptAlert,
            SkipAlert,

            // Donations
            Donation,
            DonationGoalStart,
            DonationGoalEnd,
            BitDonation,

            // Subscriptions
            Subscription,
            ReSubscription,
            SubscriptionGift,
            Subtember,

            // Twitch other
            Follow,
            Raid,
            Host,

            // Streamlabs media share
            MediaShare,
            SetVolume,
            MuteVolume,
            UnmuteVolume,
            PauseQueue,
            ResumeQueue,
            HideVideo,
            ShowVideo,

            // Streamlabs system events
            RecentEventsEvent,
            SessionReset,
            Streamlabels,
            StreamlabelsUnderlying,
            ChatBoxSettingsUpdate,
            EventsPanelSettingsUpdate,
            SeparatedLayout
        }

        public static MediaShareType ParseMediaShareType(string mediaShareType) {
            if (string.IsNullOrEmpty(mediaShareType)) { return MediaShareType.Default; }

            switch (mediaShareType.ToLowerInvariant()) {
                // Video controls
                case "play": return MediaShareType.Play;
                case "pause": return MediaShareType.Pause;
                case "next": return MediaShareType.Next;
                case "previous": return MediaShareType.Previous;
                case "replay": return MediaShareType.Replay;
                case "seek": return MediaShareType.Seek;

                // Alert control
                case "pop": return MediaShareType.Pop;
                case "show": return MediaShareType.Show;
                case "hide": return MediaShareType.Hide;

                // Queue moderation
                case "move": return MediaShareType.Move;
                case "accept": return MediaShareType.Accept;
                case "decline": return MediaShareType.Decline;
                case "ban": return MediaShareType.Ban;

                // General events
                case "newmaster": return MediaShareType.NewMaster;
                case "newpendingmedia": return MediaShareType.NewPendingMedia;
                case "newstreamermedia": return MediaShareType.NewStreamerMedia;

                default: throw new StreamlabsParseTypeException($"Failed to parse media share type. Default reached. Supplied string: {mediaShareType}");
            }
        }

        public enum MediaShareType {
            Default = 0, // Better than nullable

            // Video controls
            Play,
            Pause,
            Next,
            Previous,
            Replay,
            Seek,

            // Alert control
            Pop,
            Show,
            Hide,

            // Queue moderation
            Move,
            Accept,
            Decline,
            Ban,

            // General events
            NewMaster,
            NewPendingMedia,
            NewStreamerMedia
        }

        public static AlertPlayingType ParseAlertPlayingType(string alertPlayingType) {
            if (string.IsNullOrEmpty(alertPlayingType)) { return AlertPlayingType.Default; }

            switch (alertPlayingType.ToLowerInvariant()) {
                // Donations
                case "donation": return AlertPlayingType.Donation;
                case "bits": return AlertPlayingType.BitDonation;

                // Subscriptions
                case "subscription": return AlertPlayingType.Subscription;
                case "submysterygift": return AlertPlayingType.SubscriptionGift;

                // Twitch other
                case "pledge": return AlertPlayingType.Pledge;
                case "raid": return AlertPlayingType.Raid;

                default: throw new StreamlabsParseTypeException($"Failed to parse alert playing type. Default reached. Supplied string: {alertPlayingType}");
            }
        }

        public enum AlertPlayingType {
            Default = 0, // Better than nullable

            // Donations
            Donation,
            BitDonation,

            // Subscriptions
            Subscription,
            SubscriptionGift,

            // Twitch other
            Pledge,
            Raid
        }

    }

}
