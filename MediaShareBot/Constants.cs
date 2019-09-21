using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MediaShareBot {

    public static class Constants {

        /// <summary>
        /// Application name.
        /// </summary>
        public const string ApplicationDisplayName = "Media Share Bot";

        /// <summary>
        /// Application name with no spaces. 
        /// </summary>
        public const string ApplicationName = "MediaShareBot";

        /// <summary>
        /// Application creators name. 
        /// </summary>
        public const string Creator = "Xathz (Xathz#6861); https://github.com/Xathz";

        /// <summary>
        /// Discord trims double spaces in embeds, this prevents that.
        /// </summary>
        public const string DoubleSpace = " \u200B ";

        /// <summary>
        /// Zero width space.
        /// </summary>
        public const char ZeroWidthSpace = '\u200B';

        /// <summary>
        /// Date and time format used in messages.
        /// </summary>
        public const string DateTimeFormatShort = "MM/dd/yyyy hh:mm tt";

        /// <summary>
        /// Date and time format used in messages.
        /// </summary>
        public const string DateTimeFormatMedium = "MM/dd/yyyy hh:mm:ss tt";

        /// <summary>
        /// Date and time format used in messages.
        /// </summary>
        public const string DateTimeFormatFull = "MM/dd/yyyy hh:mm:ss.fff tt";

        /// <summary>
        /// Discord maximum file size is 8MB.
        /// </summary>
        public const long DiscordMaximumFileSize = 8388119;

        /// <summary>
        /// YouTube logo url, large.
        /// </summary>
        public const string YouTubeLogoUrl = "https://cdn.discordapp.com/attachments/559869208976949278/622132923708997638/youtube.png";

        /// <summary>
        /// YouTube logo url, small.
        /// </summary>
        public const string YouTubeLogoIconUrl = "https://cdn.discordapp.com/attachments/559869208976949278/624773952291602432/youtube_small.png";

        /// <summary>
        /// Colors, used for Discord embeds.
        /// </summary>
        /// <remarks>
        /// Media Share Bot #FF8447 (255, 132, 71)
        ///  StreamElements #6A45AB (106, 69, 171)
        ///      Streamlabs #31C3A2 (49, 195, 162)
        ///         YouTube #FF0000 (255, 0, 0)
        /// </remarks>
        public static (int R, int G, int B) MediaShareBotColor => (255, 132, 71);
        public static (int R, int G, int B) StreamElementsColor => (106, 69, 171);
        public static (int R, int G, int B) StreamlabsColor => (49, 195, 162);
        public static (int R, int G, int B) YouTubeColor => (255, 0, 0);

        /// <summary>
        /// Current process id.
        /// </summary>
        public static int ProcessId => Process.GetCurrentProcess().Id;

        /// <summary>
        /// Application version.
        /// </summary>
        public static string ApplicationVersion => typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        /// <summary>
        /// The application location including filename.
        /// </summary>
        public static string ApplicationLocation => typeof(Program).GetTypeInfo().Assembly.Location;

        /// <summary>
        /// The directory the application is in.
        /// </summary>
        public static string ApplicationDirectory => new FileInfo(ApplicationLocation).DirectoryName;

        /// <summary>
        /// Current executable name minus the extension.
        /// </summary>
        public static string ExecutableName => Path.GetFileNameWithoutExtension(ApplicationLocation);

        /// <summary>
        /// Working directory for the application.
        /// </summary>
        public static string WorkingDirectory => Path.Combine(ApplicationDirectory, ExecutableName);

        /// <summary>
        /// Log files for the application.
        /// </summary>
        public static string LogDirectory => Path.Combine(WorkingDirectory, "Logs");

        /// <summary>
        /// Temporary files for the application.
        /// </summary>
        public static string TemporaryDirectory => Path.Combine(WorkingDirectory, "Temp");

        /// <summary>
        /// Content files for the application.
        /// </summary>
        public static string ContentDirectory => Path.Combine(WorkingDirectory, "Content");

        /// <summary>
        /// Settings file location.
        /// </summary>
        public static string SettingsFile => Path.Combine(WorkingDirectory, $"{ExecutableName}.settings");

        /// <summary>
        /// Process id file location.
        /// </summary>
        public static string ProcessIdFile => Path.Combine(WorkingDirectory, $"{ExecutableName}.pid");

    }

}
