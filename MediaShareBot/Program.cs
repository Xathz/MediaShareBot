using System;
using System.IO;
using System.Threading.Tasks;
using Humanizer.Configuration;
using Humanizer.DateTimeHumanizeStrategy;
using MediaShareBot.Clients.Discord;
using MediaShareBot.Clients.Streamlabs;
using MediaShareBot.Extensions;
using MediaShareBot.Settings;

namespace MediaShareBot {

    class Program {

        private readonly DateTime _Started = DateTime.Now;
        private bool _IsExiting = false;

        static void Main() => new Program().StartAsync().GetAwaiter().GetResult();

        public Program() {
            CreatePIDFile();
            Console.Title = Constants.ApplicationDisplayName;

            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("========================================================");
            Console.WriteLine($"= {(Constants.ApplicationDisplayName + " v" + Constants.ApplicationVersion).PadBoth(52)} =");
            Console.WriteLine("= https://github.com/Xathz - https://mediasharebot.com =");
            Console.WriteLine("========================================================");
            Console.ForegroundColor = originalColor;

            Console.WriteLine();

            // Capture control c
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                BeginExit();
            };

            Directory.CreateDirectory(Constants.WorkingDirectory);
            Directory.CreateDirectory(Constants.LogDirectory);
            Directory.CreateDirectory(Constants.TemporaryDirectory);
            Directory.CreateDirectory(Constants.ContentDirectory);

            LoggingManager.Initialize();
            SettingsManager.Load();

            // Increase humanizer's precision 
            Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: .95);
            Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(precision: .95);
        }

        private async Task StartAsync() {
            LoggingManager.Log.Info("Starting...");

            // Load content into cache
            await Cache.LoadContentAsync();

            // Connect to Discord
            await DiscordClient.ConnectAsync();

            // Connect to Streamlabs
            await StreamlabsClient.ConnectAsync();

            // Block and wait
            await UserInputAsync();
        }

        private async Task UserInputAsync() {
            WaitForInput:

            string input = await Task.Run(() => Console.ReadLine());
            if (_IsExiting) { goto WaitForInput; }

            if (input == "exit") {
                BeginExit();

            } else if (input == "cache") {
                LoggingManager.Log.Info($"Keys in the cache: {Cache.ListKeys()}");

            } else if (input == "help" || input == string.Empty) {
                Console.WriteLine($"== {Constants.ApplicationDisplayName} v{Constants.ApplicationVersion}; Running for: {DateTime.Now.Subtract(_Started).ToString("c")}");
                Console.WriteLine($"== Available commands: exit, cache, help");
            }

            goto WaitForInput;
        }

        private async void BeginExit() {
            _IsExiting = true;
            LoggingManager.Log.Info("Exiting, this will take a few seconds...");

            await DiscordClient.StopAsync();

            await StreamlabsClient.StopAsync();

            LoggingManager.Flush();
            await Task.Delay(3000);

            Environment.Exit(0);
        }

        private void CreatePIDFile() {
            try {
                File.WriteAllText(Constants.ProcessIdFile, Constants.ProcessId.ToString());
            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

    }

}
