using MediaShareBot.Exceptions;

namespace MediaShareBot.Clients.Streamlabs {

    public static class Enums {

        public static EventType ParseEventType(string responseType) {
            switch (responseType.ToLowerInvariant()) {
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

                default: throw new StreamlabsParseTypeException($"Failed to parse response type. Default reached. Supplied string: {responseType}");
            }
        }

        public enum EventType {
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

        public enum MediaShareType {
            Play,
            Pause,
            Next,
            Previous,
            Replay,
            Seek,
            NewMaster,
            NewPendingMedia,
            NewStreamerMedia,
            Pop,
            Show,
            Hide,
            Move,
            Accept,
            Decline,
            Ban
        }

        public enum AlertPlayingType {
            Donation,
            BitDonation,
            Subscription,
            SubscriptionGift,
            Pledge,
            Raid
        }

    }

}
