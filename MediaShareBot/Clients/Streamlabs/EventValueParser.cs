using System;
using MediaShareBot.Extensions;
using Newtonsoft.Json.Linq;
using static MediaShareBot.Clients.Streamlabs.Enums;

namespace MediaShareBot.Clients.Streamlabs {

    public class EventValueParser {

        /// <summary>
        /// Parse a Streamlabs event for values.
        /// </summary>
        public EventValueParser(string eventText) {
            JObject eventObject = JObject.Parse(eventText);

            { // Created at date time
                string[] keys = new string[] { "createdAt", "created_at" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (DateTime.TryParse(value, out DateTime result)) { DateTime = result; break; }
                }
            }

            // Types
            EventType = ParseEventType(eventObject.Value<string>("type"));
            MediaShareType = ParseMediaShareType(eventObject.FindValueByKey<string>("event"));
            AlertPlayingType = ParseAlertPlayingType(eventObject.FindValueByKey<string>("action"));

            { // From user
                string[] keys = new string[] { "from", "gifter_display_name", "gifter", "display_name", "name" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) { FromUser = value; break; }
                }
            }

            { // Message
                string value = eventObject.FindValueByKey<string>("message");
                if (!string.IsNullOrEmpty(value)) {
                    Message = value.Trim();
                    MessageFormatted = $"```{Message.RemoveCheermotes().SanitizeForMarkdown()}```";
                }
            }

            { // Amount
                string value = eventObject.FindValueByKey<string>("amount");
                if (decimal.TryParse(value, out decimal result)) { Amount = result; }
            }

            { // Currency
                if (EventType == EventType.BitDonation) {
                    Currency = "BITS";
                } else {
                    string value = eventObject.FindValueByKey<string>("currency");
                    if (!string.IsNullOrEmpty(value)) { Currency = value; }
                }
            }

            { // Amount formatted
                string[] keys = new string[] { "formattedAmount", "formatted_amount" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) { AmountFormatted = value; break; }
                }
            }

            { // Months
                string value = eventObject.FindValueByKey<string>("months");
                if (int.TryParse(value, out int result)) { Months = result; }
            }

            { // Subscription plan
                string value = eventObject.FindValueByKey<string>("sub_plan");
                if (!string.IsNullOrEmpty(value)) {
                    if (Cache.TwitchSubscriptionPlans.TryGetValue(value, out string result)) { SubscriptionPlan = result; }
                }
            }

            { // Raiders
                string value = eventObject.FindValueByKey<string>("raiders", "0");
                if (int.TryParse(value, out int result)) { Raiders = result; }
            }

            { // Is the subscription a gift?
                IsSubscriptionGift = (eventObject.FindValueByKey<string>("sub_type") == "subgift") ? true : false;
            }

            { // Media id
                string value = eventObject.FindValueByParentAndKey<string>("media", "id");
                if (!string.IsNullOrEmpty(value)) {
                    MediaId = value;
                    IsMediaDonation = true;
                } else {
                    IsMediaDonation = false;
                }
            }

            { // Media title
                string value = eventObject.FindValueByParentAndKey<string>("media", "title");
                if (!string.IsNullOrEmpty(value)) { MediaTitle = value; }
            }

            { // Media views
                string value = eventObject.FindValueByParentAndKey<string>("statistics", "viewCount", "0");
                if (ulong.TryParse(value, out ulong result)) { MediaViews = result; }
            }

            { // Media start time
                string value = eventObject.FindValueByParentAndKey<string>("media", "start_time", "0");
                if (int.TryParse(value, out int result)) { MediaStartTime = result; }

                MediaUrl = $"https://www.youtube.com/watch?v={MediaId}&t={MediaStartTime}";
            }

            { // Media channel id
                string value = eventObject.FindValueByParentAndKey<string>("snippet", "channelId");
                if (!string.IsNullOrEmpty(value)) {
                    MediaChannelId = value;
                    MediaChannelUrl = $"https://www.youtube.com/channel/{MediaChannelId}";
                }
            }

            { // Media channel title
                string value = eventObject.FindValueByParentAndKey<string>("snippet", "channelTitle");
                if (!string.IsNullOrEmpty(value)) { MediaChannelTitle = value; }
            }

            { // Media thumbnail url
                string maxRes = eventObject.FindValueByParentAndKey<string>("maxres", "url"); // 1280x720
                string standardRes = eventObject.FindValueByParentAndKey<string>("standard", "url"); // 640x480
                string highRes = eventObject.FindValueByParentAndKey<string>("high", "url"); // 480x360
                string mediumRes = eventObject.FindValueByParentAndKey<string>("medium", "url"); // 320x180
                string defaultRes = eventObject.FindValueByParentAndKey<string>("default", "url"); // 120x90

                if (!string.IsNullOrEmpty(maxRes)) {
                    MediaThumbnailUrl = maxRes;
                } else if (!string.IsNullOrEmpty(standardRes)) {
                    MediaThumbnailUrl = standardRes;
                } else if (!string.IsNullOrEmpty(highRes)) {
                    MediaThumbnailUrl = highRes;
                } else if (!string.IsNullOrEmpty(mediumRes)) {
                    MediaThumbnailUrl = mediumRes;
                } else if (!string.IsNullOrEmpty(defaultRes)) {
                    MediaThumbnailUrl = defaultRes;
                } else {
                    MediaThumbnailUrl = Constants.YouTubeLogoUrl;
                }
            }

            { // Event log id
                string value = eventObject.FindValueByKey<string>("id");
                if (!string.IsNullOrEmpty(value)) { EventLogId = value; }
            }

            { // Event log _id
                string value = eventObject.FindValueByKey<string>("_id");
                if (!string.IsNullOrEmpty(value)) { EventLogUnderscoreId = value; }
            }

            RawJson = eventText;

        }

        public DateTime DateTime { get; private set; }

        public EventType EventType { get; private set; }

        public MediaShareType MediaShareType { get; private set; }

        public AlertPlayingType AlertPlayingType { get; private set; }

        public string FromUser { get; private set; }

        public string Message { get; private set; }

        public string MessageFormatted { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public string AmountFormatted { get; private set; }

        public int Months { get; private set; }

        public string SubscriptionPlan { get; private set; }

        public int Raiders { get; private set; }

        public bool IsSubscriptionGift { get; private set; }

        public bool IsMediaDonation { get; private set; }

        public string MediaUrl { get; private set; }

        public string MediaId { get; private set; }

        public string MediaTitle { get; private set; }

        public ulong MediaViews { get; private set; }

        public int MediaStartTime { get; private set; }

        public string MediaChannelUrl { get; private set; }

        public string MediaChannelId { get; private set; }

        public string MediaChannelTitle { get; private set; }

        public string MediaThumbnailUrl { get; private set; }

        public string EventLogId { get; private set; }

        public string EventLogUnderscoreId { get; private set; }

        public string RawJson { get; private set; }

    }

}

