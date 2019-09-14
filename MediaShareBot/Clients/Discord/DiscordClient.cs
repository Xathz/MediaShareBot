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

        private static DiscordSocketConfig _DiscordConfig;
        private static DiscordSocketClient _DiscordClient;

        private static readonly CommandServiceConfig _ServiceConfig = new CommandServiceConfig() {
            DefaultRunMode = RunMode.Async,
            LogLevel = LogSeverity.Info
        };

        private static IServiceProvider _MentionProvider;
        private static readonly CommandService _MentionService = new CommandService(_ServiceConfig);

        public static async Task ConnectAsync() {
            _DiscordConfig = new DiscordSocketConfig {
                DefaultRetryMode = RetryMode.AlwaysRetry,
                ExclusiveBulkDelete = true,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Info
            };

            _DiscordClient = new DiscordSocketClient(_DiscordConfig);
            _DiscordClient.Log += Log;

            _MentionProvider = new ServiceCollection().AddSingleton(_DiscordClient)
                .AddSingleton(_MentionService)
                .AddSingleton<MentionCommands>()
                .BuildServiceProvider();

            _MentionProvider.GetRequiredService<CommandService>().Log += Log;
            _MentionService.Log += Log;

            _DiscordClient.Connected += Connected;
            _DiscordClient.JoinedGuild += JoinedGuild;
            _DiscordClient.GuildAvailable += GuildAvailable;
            _DiscordClient.GuildMembersDownloaded += GuildMembersDownloaded;
            _DiscordClient.MessageReceived += MessageReceived;

            await _MentionProvider.GetRequiredService<MentionCommands>().InitializeAsync();

            try {
                await _DiscordClient.LoginAsync(TokenType.Bot, SettingsManager.Configuration.DiscordToken);
                await _DiscordClient.StartAsync();
            } catch (Exception ex) {
                LoggingManager.Log.Fatal(ex);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Disconnect from Discord.
        /// </summary>
        public static async Task StopAsync() {
            try {
                if (_DiscordClient.ConnectionState == ConnectionState.Connected) {
                    await _DiscordClient.LogoutAsync();
                    await _DiscordClient.StopAsync();
                }

                _DiscordClient.Dispose();
            } catch { } // Swallow
        }

        private static Task Log(LogMessage message) {
            switch (message.Severity) {
                case LogSeverity.Debug:
                    LoggingManager.Log.Info(message.Message);
                    return Task.CompletedTask;

                case LogSeverity.Verbose:
                    LoggingManager.Log.Info(message.Message);
                    return Task.CompletedTask;

                case LogSeverity.Info:
                    LoggingManager.Log.Info(message.Message);
                    return Task.CompletedTask;

                case LogSeverity.Warning:
                    LoggingManager.Log.Warn(message.Message);
                    return Task.CompletedTask;

                case LogSeverity.Error:
                    LoggingManager.Log.Error(message.Message);
                    return Task.CompletedTask;

                case LogSeverity.Critical:
                    LoggingManager.Log.Fatal(message.Message);
                    return Task.CompletedTask;

                default:
                    LoggingManager.Log.Info($"UnknownSeverity: {message.Message}");
                    return Task.CompletedTask;
            }
        }

        private static async Task Connected() {
            try {
                await _DiscordClient.CurrentUser.ModifyAsync(x => x.Username = SettingsManager.Configuration.BotNickname);
                LoggingManager.Log.Info($"Changed the bot nickname to: {SettingsManager.Configuration.BotNickname}");

                await _DiscordClient.SetGameAsync(SettingsManager.Configuration.BotPlaying);
                LoggingManager.Log.Info($"Changed the bot 'now playing' status to: {SettingsManager.Configuration.BotPlaying}");

            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        private static Task JoinedGuild(SocketGuild guild) {
            LoggingManager.Log.Info($"Joined guild {guild.Name} ({guild.Id})");
            return Task.CompletedTask;
        }

        private static Task GuildAvailable(SocketGuild guild) {
            LoggingManager.Log.Info($"Guild {guild.Name} ({guild.Id}) has become available");
            return Task.CompletedTask;
        }

        private static Task GuildMembersDownloaded(SocketGuild guild) {
            LoggingManager.Log.Info($"Full memberlist was downloaded for {guild.Name} ({guild.Id}); {guild.Users.Count.ToString("N0")} total members ({guild.Users.Where(x => x.Status == UserStatus.Online).Count().ToString("N0")} online)");
            return Task.CompletedTask;
        }

        private static async Task MessageReceived(SocketMessage socketMessage) {
            if (!(socketMessage is SocketUserMessage message)) { return; }
            if (socketMessage.Source != MessageSource.User) { return; }

            if (message.Channel is IPrivateChannel) {
                await message.Channel.SendMessageAsync($"No commands are available via direct message. Please mention the bot (`@{_DiscordClient.CurrentUser.Username}`) on the server." +
                    $"```{Constants.ApplicationDisplayName} v{Constants.ApplicationVersion}{Environment.NewLine}By {Constants.Creator}```");
            }
        }

    }

}
