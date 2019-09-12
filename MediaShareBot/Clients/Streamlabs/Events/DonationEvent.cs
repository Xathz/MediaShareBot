using System;
using MediaShareBot.Extensions;
using Newtonsoft.Json.Linq;
using static MediaShareBot.Clients.Streamlabs.Enums;

namespace MediaShareBot.Clients.Streamlabs.Events {

    public static class DonationEvent {

        public static void Process(JObject eventObject) {

            string from = eventObject.FindValueByKey<string>("from");
            string message = eventObject.FindValueByKey<string>("message");
            string amount = eventObject.FindValueByKey<string>("formattedAmount");

            string mediaId = eventObject.FindValueByParentAndKey<string>("media", "id");
            string mediaTitle = eventObject.FindValueByParentAndKey<string>("media", "title");

            string mediaChannelId = eventObject.FindValueByParentAndKey<string>("snippet", "channelId");
            string mediaChannelTitle = eventObject.FindValueByParentAndKey<string>("snippet", "channelTitle");

        }

    }

}
