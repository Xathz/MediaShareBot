using MediaShareBot.Extensions;
using Newtonsoft.Json.Linq;
using static MediaShareBot.Clients.Streamlabs.Enums;

namespace MediaShareBot.Clients.Streamlabs {

    public class EventValueParser {

        private readonly JObject _EventObject;

        /// <summary>
        /// Parse a Streamlabs event for values. All values are sanitized for markdown.
        /// </summary>
        public EventValueParser(string eventText) => _EventObject = JObject.Parse(eventText);

        private EventType _Type = EventType.Default;
        /// <summary>
        /// Event type.
        /// </summary>
        public EventType Type {
            get {
                if (_Type != EventType.Default) { return _Type; }

                _Type = ParseEventType(_EventObject.Value<string>("type"));
                return _Type;
            }
        }

        private string _From = null;
        /// <summary>
        /// From user.
        /// </summary>
        public string From {
            get {
                if (_From != null) { return _From; }
                string[] keys = new string[] { "from", "display_name", "name" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _From ?? (_From = value.SanitizeForMarkdown());
                    }
                }

                return string.Empty;
            }
        }

        private string _FromUserId = null;
        /// <summary>
        /// From user id (can be incorrect).
        /// </summary>
        public string FromUserId {
            get {
                if (_FromUserId != null) { return _FromUserId; }
                string[] keys = new string[] { "from_user_id" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _FromUserId ?? (_FromUserId = value);
                    }
                }

                return string.Empty;
            }
        }

        private string _Message = null;
        /// <summary>
        /// User message.
        /// </summary>
        public string Message {
            get {
                if (_Message != null) { return _Message; }
                string[] keys = new string[] { "message" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _Message ?? (_Message = value.RemoveCheermotes().SanitizeForMarkdown());
                    }
                }

                return string.Empty;
            }
        }

        private string _MessageCodeBlock = null;
        /// <summary>
        /// User message formatted in a markdown code block.
        /// </summary>
        public string MessageCodeBlock {
            get {
                if (_MessageCodeBlock != null) { return _MessageCodeBlock; }

                if (!string.IsNullOrEmpty(Message)) {
                    return _MessageCodeBlock ?? (_MessageCodeBlock = $"```{Message}```");
                }

                return string.Empty;
            }
        }

        private decimal? _Amount = null;
        /// <summary>
        /// Amount.
        /// </summary>
        public decimal Amount {
            get {
                if (_Amount.HasValue) { return _Amount.Value; }
                string[] keys = new string[] { "amount" };

                foreach (string key in keys) {
                    decimal? value = _EventObject.FindValueByKey<decimal>(key);
                    if (value.HasValue) {
                        return _Amount ?? (_Amount = value).Value;
                    }
                }

                return 0;
            }
        }

        private string _AmountFormatted = null;
        /// <summary>
        /// Formatted amount.
        /// </summary>
        public string AmountFormatted {
            get {
                if (_AmountFormatted != null) { return _AmountFormatted; }
                string[] keys = new string[] { "formattedAmount", "formatted_amount" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _AmountFormatted ?? (_AmountFormatted = value.SanitizeForMarkdown());
                    }
                }

                return string.Empty;
            }
        }

        private int? _Months = null;
        /// <summary>
        /// Month(s) subscribed.
        /// </summary>
        public int Months {
            get {
                if (_Months.HasValue) { return _Months.Value; }
                string[] keys = new string[] { "months" };

                foreach (string key in keys) {
                    int? value = _EventObject.FindValueByKey<int>(key);
                    if (value.HasValue) {
                        return _Months ?? (_Months = value).Value;
                    }
                }

                return 0;
            }
        }

        private string _SubscriptionPlan = null;
        /// <summary>
        /// Subscription plan.
        /// </summary>
        public string SubscriptionPlan {
            get {
                if (_SubscriptionPlan != null) { return _SubscriptionPlan; }
                string[] keys = new string[] { "sub_plan" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) {
                        if (Cache.TwitchSubscriptionPlans.TryGetValue(value, out string planLookup)) {
                            return _SubscriptionPlan ?? (_SubscriptionPlan = planLookup);
                        }
                    }
                }

                return string.Empty;
            }
        }

        #region Media Share

        /// <summary>
        /// Does this object contain media?
        /// </summary>
        public bool ContainsMedia() => !string.IsNullOrEmpty(MediaId);

        private string _MediaId = null;
        /// <summary>
        /// Id of the media.
        /// </summary>
        public string MediaId {
            get {
                if (_MediaId != null) { return _MediaId; }
                string[] keys = new string[] { "id" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByParentAndKey<string>("media", key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _MediaId ?? (_MediaId = value);
                    }
                }

                return string.Empty;
            }
        }

        private string _MediaTitle = null;
        /// <summary>
        /// Media title.
        /// </summary>
        public string MediaTitle {
            get {
                if (_MediaTitle != null) { return _MediaTitle; }
                string[] keys = new string[] { "title" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByParentAndKey<string>("media", key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _MediaTitle ?? (_MediaTitle = value.SanitizeForMarkdown());
                    }
                }

                return string.Empty;
            }
        }

        private int? _MediaStartTime = null;
        /// <summary>
        /// Media start time.
        /// </summary>
        public int MediaStartTime {
            get {
                if (_MediaStartTime.HasValue) { return _MediaStartTime.Value; }
                string[] keys = new string[] { "start_time" };

                foreach (string key in keys) {
                    int? value = _EventObject.FindValueByParentAndKey<int>("media", key);
                    if (value.HasValue) {
                        return _MediaStartTime ?? (_MediaStartTime = value).Value;
                    }
                }

                return 0;
            }
        }

        private string _MediaChannelId = null;
        /// <summary>
        /// Media channel id.
        /// </summary>
        public string MediaChannelId {
            get {
                if (_MediaChannelId != null) { return _MediaChannelId; }
                string[] keys = new string[] { "channelId" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByParentAndKey<string>("snippet", key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _MediaChannelId ?? (_MediaChannelId = value.SanitizeForMarkdown());
                    }
                }

                return string.Empty;
            }
        }

        private string _MediaChannelTitle = null;
        /// <summary>
        /// Media channel title.
        /// </summary>
        public string MediaChannelTitle {
            get {
                if (_MediaChannelTitle != null) { return _MediaChannelTitle; }
                string[] keys = new string[] { "channelTitle" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByParentAndKey<string>("snippet", key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _MediaChannelTitle ?? (_MediaChannelTitle = value.SanitizeForMarkdown());
                    }
                }

                return string.Empty;
            }
        }

        private string _MediaThumbnailUrl = null;
        /// <summary>
        /// Best available media thumbnail url.
        /// </summary>
        public string MediaThumbnailUrl {
            get {
                if (_MediaThumbnailUrl != null) { return _MediaThumbnailUrl; }

                // 1280x720
                string maxRes = _EventObject.FindValueByParentAndKey<string>("maxres", "url");
                if (!string.IsNullOrEmpty(maxRes)) { return _MediaThumbnailUrl ?? (_MediaThumbnailUrl = maxRes); }

                // 640x480
                string standardRes = _EventObject.FindValueByParentAndKey<string>("standard", "url");
                if (!string.IsNullOrEmpty(standardRes)) { return _MediaThumbnailUrl ?? (_MediaThumbnailUrl = standardRes); }

                // 480x360
                string highRes = _EventObject.FindValueByParentAndKey<string>("high", "url");
                if (!string.IsNullOrEmpty(highRes)) { return _MediaThumbnailUrl ?? (_MediaThumbnailUrl = highRes); }

                // 320x180
                string mediumRes = _EventObject.FindValueByParentAndKey<string>("medium", "url");
                if (!string.IsNullOrEmpty(mediumRes)) { return _MediaThumbnailUrl ?? (_MediaThumbnailUrl = mediumRes); }

                // 120x90
                string defaultRes = _EventObject.FindValueByParentAndKey<string>("default", "url");
                if (!string.IsNullOrEmpty(defaultRes)) { return _MediaThumbnailUrl ?? (_MediaThumbnailUrl = defaultRes); }

                return Constants.YouTubeLogoUrl;
            }
        }

        #endregion

        #region Event Log / Debug

        private string _EventLogId = null;
        public string EventLogId {
            get {
                if (_EventLogId != null) { return _EventLogId; }
                string[] keys = new string[] { "id" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _EventLogId ?? (_EventLogId = value);
                    }
                }

                return string.Empty;
            }
        }

        private string _EventLogUnderscoreId = null;
        public string EventLogUnderscoreId {
            get {
                if (_EventLogUnderscoreId != null) { return _EventLogUnderscoreId; }
                string[] keys = new string[] { "_id" };

                foreach (string key in keys) {
                    string value = _EventObject.FindValueByKey<string>(key);
                    if (!string.IsNullOrEmpty(value)) {
                        return _EventLogUnderscoreId ?? (_EventLogUnderscoreId = value);
                    }
                }

                return string.Empty;
            }
        }

        #endregion

    }

}
