using MediaShareBot.Extensions;
using Newtonsoft.Json.Linq;
using static MediaShareBot.Clients.Streamlabs.Enums;

namespace MediaShareBot.Clients.Streamlabs {

    public class EventValueParser {

        /// <summary>
        /// Parse a Streamlabs event for values. All values are sanitized for markdown.
        /// </summary>
        public EventValueParser(string eventText) {
            JObject eventObject = JObject.Parse(eventText);

            // Event type
            Type = ParseEventType(eventObject.Value<string>("type"));

            { // From user
                string[] keys = new string[] { "from", "gifter_display_name", "gifter", "display_name", "name" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) { FromUser = value; }
                }
            }

            { // From user id
                string[] keys = new string[] { "from_user_id" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) { FromUserId = value; }
                }
            }

            { // Message
                string[] keys = new string[] { "message" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) { Message = value; }
                }

                MessageFormatted = !string.IsNullOrWhiteSpace(Message) ? $"```{Message}```" : Message;
            }

            { // Amount
                string[] keys = new string[] { "amount" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (decimal.TryParse(value, out decimal decValue)) { Amount = decValue; }
                }
            }

            { // Amount formatted
                string[] keys = new string[] { "formattedAmount", "formatted_amount" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) { AmountFormatted = value; }
                }
            }

            { // Months
                string[] keys = new string[] { "months" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (int.TryParse(value, out int intValue)) { Months = intValue; }
                }
            }

            { // Subscription plan
                string[] keys = new string[] { "sub_plan" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) {
                        if (Cache.TwitchSubscriptionPlans.TryGetValue(value, out string plan)) { SubscriptionPlan = plan; }
                    }
                }
            }

            { // Is the subscription a gift?
                IsSubscriptionGift = (eventObject.FindValueByKey<string>("sub_type") == "subgift") ? true : false;
            }

            { // Media id
                string[] keys = new string[] { "id" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByParentAndKey<string>("media", key);
                    if (!string.IsNullOrEmpty(value)) { MediaId = value; }
                }

                IsMediaDonation = !string.IsNullOrEmpty(MediaId) ? true : false;
            }

            { // Media title
                string[] keys = new string[] { "title" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByParentAndKey<string>("media", key);
                    if (!string.IsNullOrEmpty(value)) { MediaTitle = value; }
                }
            }

            { // Media start time
                string[] keys = new string[] { "start_time" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByParentAndKey<string>("media", key);
                    if (int.TryParse(value, out int intValue)) { MediaStartTime = intValue; }
                }
            }

            { // Media channel id
                string[] keys = new string[] { "channelId" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByParentAndKey<string>("snippet", key);
                    if (!string.IsNullOrEmpty(value)) { MediaChannelId = value; }
                }
            }

            { // Media channel title
                string[] keys = new string[] { "channelTitle" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByParentAndKey<string>("snippet", key);
                    if (!string.IsNullOrEmpty(value)) { MediaChannelTitle = value; }
                }
            }

            { // Event log id
                string[] keys = new string[] { "id" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) { EventLogId = value; }
                }
            }

            { // Event log _id
                string[] keys = new string[] { "_id" };

                foreach (string key in keys) {
                    string value = eventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) { EventLogUnderscoreId = value; }
                }
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

        }

        public EventType Type { get; private set; }

        public string FromUser { get; private set; }

        public string FromUserId { get; private set; }

        public string Message { get; private set; }

        public string MessageFormatted { get; private set; }

        public decimal Amount { get; private set; }

        public string AmountFormatted { get; private set; }

        public int Months { get; private set; }

        public string SubscriptionPlan { get; private set; }

        public bool IsSubscriptionGift { get; private set; }

        public string MediaId { get; private set; }

        public bool IsMediaDonation { get; private set; }

        public string MediaTitle { get; private set; }

        public int MediaStartTime { get; private set; }

        public string MediaChannelId { get; private set; }

        public string MediaChannelTitle { get; private set; }

        public string EventLogId { get; private set; }

        public string EventLogUnderscoreId { get; private set; }

        public string MediaThumbnailUrl { get; private set; }

    }

}

