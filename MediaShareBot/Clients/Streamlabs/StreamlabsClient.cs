using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Humanizer;
using MediaShareBot.Clients.Streamlabs.Events;
using MediaShareBot.Exceptions;
using MediaShareBot.Extensions;
using MediaShareBot.Settings;
using MediaShareBot.Utilities;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using static MediaShareBot.Clients.Streamlabs.Enums;

namespace MediaShareBot.Clients.Streamlabs {

    public static class StreamlabsClient {

        private const int _Timeout = 5000;
        private static int _ReconnectAttempts = 4;
        private static int _ReconnectBackoff = 5000;
        private static bool _InitialConnection = true;
        private static bool _IsExiting = false;

        private static readonly SocketIO _Client = new SocketIO("https://aws-io.streamlabs.com");

        private static async Task GetSocketToken() {
            try {
                LoggingManager.Log.Info("Downloading socket token");

                Task<string> download = Http.SendRequestAsync($"https://streamlabs.com/api/v5/io/info?token={SettingsManager.Configuration.StreamlabsToken}",
                    new Dictionary<string, string> {
                        { "Referer", "https://streamlabs.com" }
                    });

                JObject downloadedJson = JObject.Parse(await download);
                string path = downloadedJson.Value<string>("path");

                // Socket token
                string token = Http.ParseUrlForParameter(path, "token");
                if (!string.IsNullOrEmpty(token)) {
                    _Client.Parameters = new Dictionary<string, string> {
                        { "token", token }
                    };
                } else {
                    throw new ArgumentNullException("The token is null or empty.");
                }

                // Socket timeout
                int timeout = downloadedJson.FindValueByKey<int>("timeout");
                _Client.ConnectTimeout = new TimeSpan(0, 0, 0, 0, timeout > _Timeout ? timeout : _Timeout);

                // Socket reconnection attempts
                int reconnectAttempts = downloadedJson.FindValueByKey<int>("reconnect_attempts");
                if (reconnectAttempts > _ReconnectAttempts) {
                    _ReconnectAttempts = reconnectAttempts;
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        private static void SetHandlers() {
            if (_InitialConnection) {
                _Client.OnConnected += SocketConnected;
                _Client.OnClosed += SocketClosedAsync;

                _Client.On("event", response => {
                    Task runner = Task.Run(async () => {
                        await ProcessEvent(response.Text);
                    });
                });

                _InitialConnection = false;
            }
        }

        public static async Task ConnectAsync() {
            try {
                LoggingManager.Log.Info("Connecting");

                SetHandlers();

                Task getToken = GetSocketToken();
                await getToken;

                if (getToken.IsCompletedSuccessfully) {
                    await _Client.ConnectAsync();
                } else {
                    throw getToken.Exception;
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        public static async Task StopAsync() {
            try {
                _IsExiting = true;
                LoggingManager.Log.Info("Disconnecting");

                if (_Client.State == SocketIOState.Connected) {
                    await _Client.CloseAsync();
                }

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        private static void SocketConnected() => LoggingManager.Log.Info("Connected");

        private static async void SocketClosedAsync(ServerCloseReason reason) {
            if (_IsExiting) { return; }
            LoggingManager.Log.Warn($"Disconnected: {reason.Humanize(LetterCasing.Sentence)}");

            for (int i = 0; i < _ReconnectAttempts; i++) {
                LoggingManager.Log.Info($"Reconnecting... attempt {i}/{_ReconnectAttempts}");

                try {
                    Task connect = ConnectAsync();
                    await connect;

                    if (connect.IsCompletedSuccessfully) {
                        _ReconnectBackoff = 5000;
                        break;
                    } else {
                        throw connect.Exception;
                    }

                } catch (Exception ex) {
                    LoggingManager.Log.Error(ex, $"Failed to reconnect: {ex.Message}");

                    LoggingManager.Log.Info("Waiting a few seconds");
                    await Task.Delay(_ReconnectBackoff);

                    _ReconnectBackoff += 2000;
                }
            }

            LoggingManager.Log.Fatal("All reconnection attempts have failed, exiting");
            Environment.Exit(1);
        }

        private static async Task ProcessEvent(string eventText) {

#if DEBUG
            Console.WriteLine($"{DateTime.Now.ToString(Constants.DateTimeFormatFull)}: {eventText}");
            Console.WriteLine("================");
#endif

            try {
                EventValueParser parser = new EventValueParser(eventText);

                if (parser.Type == EventType.Donation) {
                    await new DonationEvent(parser).Process();

                } else if (parser.Type == EventType.BitDonation) {
                    await new BitDonationEvent(parser).Process();

                } else if (parser.Type == EventType.Subscription) {
                    await new SubscriptionEvent(parser).Process();

                } else if (parser.Type == EventType.ReSubscription) {
                    await new SubscriptionEvent(parser).Process();

                } else if (parser.Type == EventType.SubscriptionGift) {
                    await new SubscriptionGiftEvent(parser).Process();

                }

            } catch (StreamlabsParseTypeException ex) {
                LoggingManager.Log.Warn(ex);
            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

    }

}
